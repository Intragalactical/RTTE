using LanguageExt;
using RTTE.Library.Common.Interfaces;
using System;
using DiagnosticsProcess = System.Diagnostics.Process;

namespace RTTE.Library.Process;

public sealed class BaseProcess : IProcess {
    private DiagnosticsProcess Process { get; }

    private BaseProcess(DiagnosticsProcess process) {
        Process = process;
    }

    public Option<IntPtr> GetHandle() => Process.Handle;
    public int GetId() => Process.Id;
    public string GetName() => Process.ProcessName;
    public Option<string> GetMainWindowTitle() => Process.MainWindowTitle;
    public IntPtr GetMainWindowHandle() => Process.MainWindowHandle;

    public static IProcess Create(DiagnosticsProcess process)
        => new BaseProcess(process);
}
