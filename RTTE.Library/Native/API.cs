using RTTE.Library.Common;
using RTTE.Library.Common.Interfaces;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace RTTE.Library.Native;

/**
 * NOTE:
 * The following class should ONLY contain external functions from DllImports,
 * and the most basic instanced, public versions of them.
 * At no point should there be a function that does something else than
 * immediately call a private static external function!
 * As such, this class is excluded from coverage.
 **/
[ExcludeFromCoverage]
public sealed class API : IWinAPI {
    [DllImport("user32.dll", EntryPoint = "GetWindowRect", SetLastError = true)]
    private static extern bool GetWindowRectNative(IntPtr hwnd, out NativeRectangle lpRect);

    [DllImport("user32.dll", EntryPoint = "PrintWindow", SetLastError = true)]
    private static extern bool PrintWindowNative(IntPtr hWnd, IntPtr hdcBlt, int nFlags);

    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "QueryFullProcessImageName")]
    private static extern bool QueryFullProcessImageNameNative(IntPtr hProcess, int dwFlags, StringBuilder lpExeName, ref int lpdwSize);

    [DllImport("kernel32.dll", SetLastError = true, EntryPoint = "OpenProcess")]
    private static extern IntPtr OpenProcessNative(
         uint processAccess,
         bool bInheritHandle,
         uint processId
    );

    [DllImport("kernel32.dll", EntryPoint = "VirtualQueryEx", SetLastError = true)]
    private static extern int VirtualQueryExNative(
        IntPtr hProcess,
        IntPtr lpAddress,
        out MemoryBasicInformation lpBuffer,
        uint dwLength
    );

    [DllImport("kernel32.dll", EntryPoint = "GetSystemInfo", SetLastError = true)]
    private static extern void GetSystemInfoNative(out SystemInfo lpSystemInfo);

    [DllImport("kernel32.dll", EntryPoint = "ReadProcessMemory", SetLastError = true)]
    private static extern bool ReadProcessMemoryNative(
        IntPtr hProcess,
        IntPtr lpBaseAddress,
        byte[] lpBuffer,
#if X86
            uint dwSize,
#endif
#if X64
            ulong dwSize,
#endif
            out uint lpNumberOfBytesRead
    );

    [DllImport("user32.dll", SetLastError = true, EntryPoint = "EnumDisplayMonitors")]
    private static extern bool EnumDisplayMonitorsNative(
        IntPtr hdc,
        IntPtr lprcClip,
        IWinAPI.EnumMonitorsDelegate lpfnEnum,
        IntPtr dwData
    );

    [DllImport("user32.dll", SetLastError = true, EntryPoint = "EnumDisplaySettingsEx")]
    private static extern bool EnumDisplaySettingsExNative(byte[] lpszDeviceName, int iModeNum, ref DevMode lpDevMode);

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "EnumDisplayDevices")]
    private static extern bool EnumDisplayDevicesNative(string lpDevice, uint iDevNum, ref DisplayDevice lpDisplayDevice, uint dwFlags);

    [DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "GetMonitorInfo")]
    private static extern bool GetMonitorInfoNative(IntPtr hMonitor, ref MonitorInfoEx lpmi);

    private API() { }

    public bool GetWindowRect(IntPtr hwnd, out NativeRectangle lpRect)
        => GetWindowRectNative(hwnd, out lpRect);

    public IntPtr OpenProcess(uint processAccess, bool bInheritHandle, uint processId)
        => OpenProcessNative(processAccess, bInheritHandle, processId);

    public bool PrintWindow(IntPtr windowHandle, IntPtr hdcBlt, int flags)
        => PrintWindowNative(windowHandle, hdcBlt, flags);

    public bool QueryFullProcessImageName(IntPtr hProcess, int dwFlags, StringBuilder lpExeName, ref int lpdwSize)
        => QueryFullProcessImageNameNative(hProcess, dwFlags, lpExeName, ref lpdwSize);

    public int VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out MemoryBasicInformation lpBuffer, uint dwLength)
        => VirtualQueryExNative(hProcess, lpAddress, out lpBuffer, dwLength);

    public void GetSystemInfo(out SystemInfo lpSystemInfo)
        => GetSystemInfoNative(out lpSystemInfo);

    public bool ReadProcessMemory(
        IntPtr hProcess,
        IntPtr lpBaseAddress,
        byte[] lpBuffer,
#if X86
            uint dwSize,
#endif
#if X64
            ulong dwSize,
#endif
            out uint lpNumberOfBytesRead
    ) => ReadProcessMemoryNative(hProcess, lpBaseAddress, lpBuffer, dwSize, out lpNumberOfBytesRead);

    public bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, IWinAPI.EnumMonitorsDelegate lpfnEnum, IntPtr dwData)
        => EnumDisplayMonitorsNative(hdc, lprcClip, lpfnEnum, dwData);

    public bool EnumDisplaySettingsEx(byte[] lpszDeviceName, int iModeNum, ref DevMode lpDevMode)
        => EnumDisplaySettingsExNative(lpszDeviceName, iModeNum, ref lpDevMode);

    public bool EnumDisplayDevices(string lpDevice, uint iDevNum, ref DisplayDevice lpDisplayDevice, uint dwFlags)
        => EnumDisplayDevicesNative(lpDevice, iDevNum, ref lpDisplayDevice, dwFlags);

    public bool GetMonitorInfo(IntPtr hMonitor, ref MonitorInfoEx lpmi)
        => GetMonitorInfoNative(hMonitor, ref lpmi);

    public static IWinAPI Create()
        => new API();
}
