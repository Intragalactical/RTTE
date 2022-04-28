using LanguageExt;
using RTTE.Library.Common;
using RTTE.Library.Common.Interfaces;
using RTTE.Library.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using DisplayMonitor = RTTE.Library.Display.Display;

namespace RTTE.Library.Display;

public class DisplayList : IDisplayList {
    private record struct DisplayDeviceAndDevMode(DisplayDevice DisplayDevice, DevMode DevMode);

    private IWinAPI WinAPI { get; }
    private IScreenGrabber ScreenGrabber { get; }

    private DisplayList(IWinAPI winAPI, IScreenGrabber screenGrabber) {
        WinAPI = winAPI;
        ScreenGrabber = screenGrabber;
    }

    public IEnumerable<IDisplay> Get()
        => EnumDisplayDevices(ScreenGrabber).Somes();

    private Option<MonitorInfoEx> GetMonitorInfo(IntPtr monitorHandle) {
        MonitorInfoEx infoEx = new();
        infoEx.Init();

        bool success = WinAPI.GetMonitorInfo(monitorHandle, ref infoEx);
        return success ? infoEx : Option<MonitorInfoEx>.None;
    }

    private Option<IEnumerable<string>> GetMonitorDeviceNames() {
        List<string> displayMonitorNames = new();

        bool enumDisplayMonitorsProc(
            IntPtr hMonitor,
            IntPtr hdcMonitor,
            NativeRectangle lprcMonitor,
            IntPtr dwData
        ) {
            Option<MonitorInfoEx> MonitorInfo = GetMonitorInfo(hMonitor);

            _ = MonitorInfo.Match(monitorInfo => displayMonitorNames.Add(monitorInfo.DeviceName), () => { });

            return MonitorInfo.IsSome;
        }

        bool success = WinAPI.EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, enumDisplayMonitorsProc, IntPtr.Zero);
        return success ?
            displayMonitorNames :
            Option<IEnumerable<string>>.None;
    }

    private IEnumerable<DisplayDeviceAndDevMode> FetchAllDisplayDevicesAndSettings(int displayIndex = 0) {
        DisplayDevice device = new();
        device.cb = Marshal.SizeOf(device);
        bool successDisplayDevice = WinAPI.EnumDisplayDevices(null, (uint)displayIndex, ref device, 0);

        return successDisplayDevice ?
            FetchDisplaySettings(device, displayIndex) :
            new List<DisplayDeviceAndDevMode>();
    }

    private IEnumerable<DisplayDeviceAndDevMode> FetchDisplaySettings(DisplayDevice device, int displayIndex) {
        List<DisplayDeviceAndDevMode> displayDeviceAndDevModes = new();

        DevMode devMode = new();
        devMode.dmSize = (short)Marshal.SizeOf(typeof(DevMode));
        bool success = WinAPI.EnumDisplaySettingsEx(device.DeviceName.ToLPTStr(), -1, ref devMode);

        if (success)
            displayDeviceAndDevModes.Add(new(device, devMode));

        return displayDeviceAndDevModes.Concat(FetchAllDisplayDevicesAndSettings(displayIndex + 1));
    }

    private IEnumerable<Option<IDisplay>> GetDisplayMonitors(IScreenGrabber screenGrabber, IEnumerable<string> monitorNames)
        => FetchAllDisplayDevicesAndSettings()
            .Where(displayDeviceAndDevMode => monitorNames.Any(monitorInfo => monitorInfo.Equals(
                displayDeviceAndDevMode.DisplayDevice.DeviceName,
                StringComparison.InvariantCultureIgnoreCase
            )))
            .Select((displayDeviceAndDevMode) =>
                displayDeviceAndDevMode.DevMode.GetArea().Match(
                    area => DisplayMonitor.Create(
                        screenGrabber,
                        displayDeviceAndDevMode.DisplayDevice.DeviceName,
                        area
                    ),
                    Option<IDisplay>.None
                )
            );

    private IEnumerable<Option<IDisplay>> EnumDisplayDevices(IScreenGrabber screenGrabber) {
        Option<IEnumerable<string>> monitorNames = GetMonitorDeviceNames();

        return monitorNames.Match(
            monitorNames => GetDisplayMonitors(screenGrabber, monitorNames),
            new List<Option<IDisplay>>()
        );
    }

    public static IDisplayList Create(IWinAPI winAPI, IScreenGrabber screenGrabber)
        => new DisplayList(winAPI, screenGrabber);
}
