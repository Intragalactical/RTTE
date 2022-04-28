using IOFile = System.IO.File;
using RTTE.Library.Common.Interfaces;

namespace RTTE.Library.File;

public sealed class BaseFile : IFile {
    private BaseFile() { }

    public bool Exists(string filePath) => IOFile.Exists(filePath);

    public static IFile Create() => new BaseFile();
}
