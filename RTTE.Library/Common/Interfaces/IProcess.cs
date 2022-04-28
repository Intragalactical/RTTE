using LanguageExt;
using System;

namespace RTTE.Library.Common.Interfaces;

public interface IProcess {
    public Option<string> GetMainWindowTitle();
    public IntPtr GetMainWindowHandle();
    public string GetName();
    public int GetId();
    public Option<IntPtr> GetHandle();
}
