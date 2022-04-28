using LanguageExt;
using RTTE.Library.Common;
using RTTE.Library.Common.Interfaces;
using RTTE.Library.Native;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RTTE.Library.Process;

public sealed class ProcessMemoryScanner {
    private record ScannerInfo(
        IWinAPI WinAPI,
        int ProcessId,
        AddressFound OnAddressFound,
        IEnumerable<(Encoding, byte[])> EncodingTextBytesMap,
        CancellationToken CancellationToken
    );

    public delegate void ProgressChanged(int processId, int value);
    public delegate void AddressFound(ProcessAddressHook addressHook);

    public event ProgressChanged OnProgressChanged;
    public event AddressFound OnAddressFound;

    public string ProcessName { get; }
    public int ProcessId { get; }
    public ProcessArchitecture ProcessArchitecture { get; }
    public Option<string> ProcessMainWindowTitle { get; }

    private int progress;
    private int Progress {
        get => progress;
        set {
            if (progress != value) {
                progress = value;
                OnProgressChanged?.Invoke(ProcessId, value);
            }
        }
    }

    private Option<IntPtr> ProcessHandle { get; }

    private IWinAPI WinAPI { get; }

    private ProcessMemoryScanner(
        IWinAPI winAPI,
        string processName,
        int processId,
        ProcessArchitecture processArchitecture,
        Option<string> processMainWindowTitle,
        Option<IntPtr> processHandle
    ) {
        WinAPI = winAPI;
        ProcessName = processName;
        ProcessId = processId;
        ProcessArchitecture = processArchitecture;
        ProcessMainWindowTitle = processMainWindowTitle;
        ProcessHandle = processHandle;
        Progress = 0;
    }

    public static async Task<IEnumerable<ProcessAddressHook>> Refine(
        IWinAPI winAPI,
        IEnumerable<ProcessAddressHook> addressHooks,
        string text,
        IEnumerable<Encoding> encodings
    ) => await Task.Run(() => {
        List<Option<ProcessAddressHook>> distinct = new();
        IDictionary<Encoding, byte[]> encodingTextBytesMap = encodings
            .ToDictionary(e => e, e => e.GetBytes(text));
        IEnumerable<ProcessAddressHook> searchFrom = addressHooks.Where(addressHook => encodings.Any(encoding => addressHook.TextEncoding.Equals(encoding)));

        foreach (ProcessAddressHook addressHook in searchFrom) {
            byte[] textBytes = encodingTextBytesMap[addressHook.TextEncoding];
            Option<IEnumerable<byte>> readBytes =
                ReadMemoryRegion(winAPI, addressHook.ProcessHandle, addressHook.Address, (uint)textBytes.Length);

            distinct.Add(readBytes.Match(
                readBytes =>
                    readBytes.SequenceEqual(textBytes) ?
                        addressHook :
                        Option<ProcessAddressHook>.None,
                Option<ProcessAddressHook>.None
            ));
        }

        return distinct.Somes();
    });

    private static IEnumerable<ProcessAddressHook> GetAddressesFromAddress(
        IntPtr processHandle,
        ScannerInfo scannerInfo,
        long currentAddress,
        IEnumerable<byte> readBytes,
        ulong regionSize
    ) {
        var (_, ProcessId, OnAddressFound, EncodingTextBytesMap, CancellationToken) = scannerInfo;

        ParallelOptions parallelOptions = new() { CancellationToken = CancellationToken };
        ConcurrentQueue<ProcessAddressHook> foundAddresses = new();

        var handledForEach = Utils.DisregardException<
            OperationCanceledException,
            IEnumerable<(Encoding, byte[])>,
            ParallelOptions,
            Action<(Encoding, byte[])>,
            ParallelLoopResult
        >(Parallel.ForEach);
        var handledFor = Utils.DisregardException<
#if X86
                OperationCanceledException,
                int,
                int,
                ParallelOptions,
                Action<int>,
                ParallelLoopResult
#endif
#if X64
                OperationCanceledException,
            long,
            long,
            ParallelOptions,
            Action<long>,
            ParallelLoopResult
#endif
            >(Parallel.For);

        void readNextEncoding((Encoding Encoding, byte[] Bytes) value) {
#if X86
                handledFor(0, (int)regionSize, currentIndex => {
#endif
#if X64
            handledFor(0L, (long)regionSize, parallelOptions, currentIndex => {
#endif
                IEnumerable<byte> nextCharacters = readBytes.Skip(Convert.ToInt32(currentIndex)).Take(value.Bytes.Length);

                if (!nextCharacters.SequenceEqual(value.Bytes))
                    return;

                IntPtr currentAddressWithOffset = new(currentAddress + currentIndex);
                ProcessAddressHook addressHook = new(processHandle, ProcessId, value.Encoding, currentAddressWithOffset);
                foundAddresses.Enqueue(addressHook);
                OnAddressFound?.Invoke(addressHook);
            });
        }

        handledForEach(EncodingTextBytesMap, parallelOptions, readNextEncoding);

        return foundAddresses;
    }

    private static IEnumerable<ProcessAddressHook> TryReadMemoryAndGetAddresses(
        IntPtr processHandle,
        ScannerInfo scannerInfo,
        long currentAddress,
        IntPtr baseAddress,
        ulong regionSize
    ) {
        Option<IEnumerable<byte>> readBytes =
            ReadMemoryRegion(scannerInfo.WinAPI, processHandle, baseAddress, regionSize);

        return readBytes.Match(
            readBytes => GetAddressesFromAddress(
                processHandle,
                scannerInfo,
                currentAddress,
                readBytes,
                regionSize
            ),
            new List<ProcessAddressHook>()
        );
    }

    private static (ulong, IEnumerable<ProcessAddressHook>) TryReadAddressesFromMemoryBlock(
        IntPtr processHandle,
        ScannerInfo scannerInfo,
        long currentAddress,
        MemoryBasicInformation memoryBasicInformation
    ) {
        uint protect = memoryBasicInformation.Protect;
        uint state = memoryBasicInformation.State;
        ulong regionSize = Convert.ToUInt64(memoryBasicInformation.RegionSize);

        IEnumerable<ProcessAddressHook> processAddressHooks =
            protect == MemoryProtection.PAGE_READWRITE && state == MemoryAllocationType.MEM_COMMIT ?
                TryReadMemoryAndGetAddresses(
                    processHandle,
                    scannerInfo,
                    currentAddress,
                    (IntPtr)memoryBasicInformation.BaseAddress,
                    regionSize
                ) :
                new List<ProcessAddressHook>();

        return (regionSize, processAddressHooks);
    }

    public async Task<IEnumerable<ProcessAddressHook>> Scan(
        IWinAPI winAPI,
        string text,
        IEnumerable<Encoding> encodings,
        CancellationTokenSource cancellationTokenSource
    ) => await Task.Run(() => {
        IEnumerable<(Encoding, byte[])> encodingTextBytesMap = encodings
            .Select(e => (e, e.GetBytes(text)));
        List<ProcessAddressHook> allFoundAddresses = new();

        SystemInfo systemInfo = GetSystemInfo();

        IntPtr minimumAddress = systemInfo.MinimumApplicationAddress;
        IntPtr maximumAddress = systemInfo.MaximumApplicationAddress;

        ulong regionSize = 0;

        ScannerInfo scannerInfo = new(
            winAPI,
            ProcessId,
            OnAddressFound,
            encodingTextBytesMap,
            cancellationTokenSource.Token
        );

        for (long currentAddress = (long)minimumAddress; currentAddress < (long)maximumAddress; currentAddress += (long)regionSize) {
            if (cancellationTokenSource.IsCancellationRequested)
                break;

            Progress = (int)Math.Ceiling(currentAddress / (decimal)maximumAddress * 100);

            IEnumerable<ProcessAddressHook> readAddressesAndSetRegionSize(IntPtr processHandle, MemoryBasicInformation memoryBasicInformation) {
                (ulong newRegionSize, IEnumerable<ProcessAddressHook> addresses) =
                    TryReadAddressesFromMemoryBlock(
                        processHandle,
                        scannerInfo,
                        currentAddress,
                        memoryBasicInformation
                    );
                regionSize = newRegionSize;
                return addresses;
            }

            bool stopFlag = false;

            IEnumerable<ProcessAddressHook> setFlagAndReturnEmpty() {
                stopFlag = true;
                return new List<ProcessAddressHook>();
            }

            IEnumerable<ProcessAddressHook> continueMemoryReadWithProcessHandle(IntPtr processHandle)
                => QueryMemoryInformation(processHandle, (IntPtr)currentAddress).Match(
                    memoryBasicInformation => readAddressesAndSetRegionSize(processHandle, memoryBasicInformation),
                    setFlagAndReturnEmpty
                );

            IEnumerable<ProcessAddressHook> addressHooks = ProcessHandle.Match(
                continueMemoryReadWithProcessHandle,
                new List<ProcessAddressHook>()
            );
            allFoundAddresses.AddRange(addressHooks);

            if (stopFlag)
                break;
        }

        return allFoundAddresses;
    });

    public SystemInfo GetSystemInfo() {
        WinAPI.GetSystemInfo(out SystemInfo sysInfo);
        return sysInfo;
    }

    private Option<MemoryBasicInformation> QueryMemoryInformation(
        IntPtr processHandle,
        IntPtr address
    ) {
        int returnCode = WinAPI.VirtualQueryEx(
            processHandle,
            address,
            out MemoryBasicInformation memoryBasicInformation,
            (uint)Marshal.SizeOf(typeof(MemoryBasicInformation))
        );
        return returnCode != 0 ?
            memoryBasicInformation :
            Option<MemoryBasicInformation>.None;
    }

    public static Option<IEnumerable<byte>> ReadMemoryRegion(
        IWinAPI winAPI,
        IntPtr processHandle,
        IntPtr baseAddress,
#if X86
            uint regionSize
#endif
#if X64
            ulong regionSize
#endif
        ) {
        var buffer = new byte[regionSize];
        bool success = winAPI.ReadProcessMemory(
            processHandle,
            baseAddress,
            buffer,
            regionSize,
            out uint readBytes
        );
        return success ?
            Option<IEnumerable<byte>>.Some(buffer.Take((int)readBytes)) :
            Option<IEnumerable<byte>>.None;
    }

    public object[] AsObjectArray()
        => new object[] { ProcessName, ProcessId, ProcessArchitecture, ProcessMainWindowTitle, Progress };

    public static Option<ProcessMemoryScanner> Create(IWinAPI winAPI, IAdvancedProcess advancedProcess)
        => advancedProcess.GetArchitecture().Match(
            (architecture) => new ProcessMemoryScanner(
                winAPI,
                advancedProcess.GetName(),
                advancedProcess.GetId(),
                architecture,
                advancedProcess.GetMainWindowTitle(),
                advancedProcess.GetHandle()
            ),
            Option<ProcessMemoryScanner>.None
        );
}
