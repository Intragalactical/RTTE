using LanguageExt;
using PeFileClass = PeNet.PeFile;
using System;
using RTTE.Library.Common.Interfaces;

namespace RTTE.Library.PeFile;

public sealed class PeFileParser : IPeFileParser {
    private IFile FileIO { get; }
    private Func<string, Option<IPeFile>> Parser { get; }
    public static Func<string, Option<IPeFile>> DefaultParser => file => {
        bool success = PeFileClass.TryParse(file, out PeFileClass peFile);
        return success ?
            Option<IPeFile>.Some(BasePeFile.Create(peFile)) :
            Option<IPeFile>.None;
    };

    private PeFileParser(IFile fileIO, Func<string, Option<IPeFile>> parser) {
        FileIO = fileIO;
        Parser = parser;
    }

    public Option<IPeFile> TryParse(string file) {
        return FileIO.Exists(file) ?
            Parser(file) :
            Option<IPeFile>.None;
    }

    public static IPeFileParser Create(IFile fileIO, Func<string, Option<IPeFile>> parser)
        => new PeFileParser(fileIO, parser);
}
