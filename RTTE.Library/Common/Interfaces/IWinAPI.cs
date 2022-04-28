using RTTE.Library.Native;
using System;
using System.Text;

namespace RTTE.Library.Common.Interfaces;

public interface IWinAPI {
    public delegate bool EnumMonitorsDelegate(IntPtr hMonitor, IntPtr hdcMonitor, NativeRectangle lprcMonitor, IntPtr dwData);

    public bool GetWindowRect(IntPtr hwnd, out NativeRectangle lpRect);
    public bool PrintWindow(IntPtr windowHandle, IntPtr hdcBlt, int flags);
    public bool QueryFullProcessImageName(IntPtr hProcess, int dwFlags, StringBuilder lpExeName, ref int lpdwSize);
    public IntPtr OpenProcess(uint processAccess, bool bInheritHandle, uint processId);
    public int VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out MemoryBasicInformation lpBuffer, uint dwLength);
    public void GetSystemInfo(out SystemInfo lpSystemInfo);
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
    );
    public bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, EnumMonitorsDelegate lpfnEnum, IntPtr dwData);
    public bool EnumDisplayDevices(string lpDevice, uint iDevNum, ref DisplayDevice lpDisplayDevice, uint dwFlags);
    public bool EnumDisplaySettingsEx(byte[] lpszDeviceName, int iModeNum, ref DevMode lpDevMode);
    public bool GetMonitorInfo(IntPtr hMonitor, ref MonitorInfoEx lpmi);
}
