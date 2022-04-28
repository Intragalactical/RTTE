using LanguageExt;
using RTTE.Library.Process;
using System.Drawing;

namespace RTTE.Library.Common.Interfaces;

public interface IAdvancedProcess : IProcess {
    public string GetArchitectureString();
    public Option<string> GetFullPath();
    public Option<Image> GetPreviewImage();
    public Option<ProcessArchitecture> GetArchitecture();
    public string AsString();
}
