namespace RTTE.Library.Native;

public static class ProcessAccessRight {
    public const uint AllAccess = 0x000F0000 | 0x00100000 | 0xFFF;
    public const uint VmOperation = 0x0008;
    public const uint VmWrite = 0x0020;
}
