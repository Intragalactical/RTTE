using LanguageExt;

namespace RTTE.Library.Common.Interfaces;

public interface IPeFileParser {
    public Option<IPeFile> TryParse(string file);
}
