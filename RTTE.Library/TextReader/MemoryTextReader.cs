using LanguageExt;
using RTTE.Library.Common.Interfaces;
using RTTE.Library.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RTTE.Library.TextReader;

public sealed class MemoryTextReader : ITextReader {
    private const int BufferSize = 128;

    private IWinAPI WinAPI { get; }
    private IntPtr ProcessHandle { get; }
    private IntPtr Address { get; }
    private Encoding CurrentEncoding { get; }

    public MemoryTextReader(IWinAPI winAPI, IntPtr processHandle, IntPtr address, Encoding currentEncoding) {
        WinAPI = winAPI;
        ProcessHandle = processHandle;
        Address = address;
        CurrentEncoding = currentEncoding;
    }

    public string Read() {
        static IEnumerable<byte> readNext(IWinAPI winAPI, IntPtr processHandle, IntPtr address) {
            Option<IEnumerable<byte>> readBytes =
                ProcessMemoryScanner.ReadMemoryRegion(winAPI, processHandle, address, BufferSize);

            return readBytes.Match(
                readBytes => readBytes.Concat(readNext(winAPI, processHandle, address + BufferSize)),
                new List<byte>()
            );
        }

        IEnumerable<byte> bytesFromMemory = readNext(WinAPI, ProcessHandle, Address);

        return CurrentEncoding.GetString(bytesFromMemory.ToArray()).Split('\0')[0];
    }
}
