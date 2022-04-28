namespace RTTE.Library.Process;

public record class ProcessFilterOptions(
    string Filter,
    bool HideInvalid,
    bool HideWindowless,
    ProcessArchitecture CurrentArchitecture
);
