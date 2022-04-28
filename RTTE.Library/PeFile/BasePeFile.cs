using RTTE.Library.Common.Interfaces;
using RTTE.Library.Process;
using PeFileClass = PeNet.PeFile;

namespace RTTE.Library.PeFile;

public sealed class BasePeFile : IPeFile {
    private PeFileClass PeFile { get; }

    private BasePeFile(PeFileClass peFile) {
        PeFile = peFile;
    }

    public ProcessArchitecture GetArchitecture()
        => PeFile.Is64Bit ? ProcessArchitecture.x64 : ProcessArchitecture.x86;

    public static IPeFile Create(PeFileClass peFile)
        => new BasePeFile(peFile);
}
