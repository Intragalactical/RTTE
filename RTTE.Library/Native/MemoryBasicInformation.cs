namespace RTTE.Library.Native;

public struct MemoryBasicInformation {
#if X86
    public uint BaseAddress;
    public uint AllocationBase;
    public uint AllocationProtect;
    public uint RegionSize;   // size of the region allocated by the program
    public uint State;   // check if allocated (MEM_COMMIT)
    public uint Protect; // page protection (must be PAGE_READWRITE)
    public uint Type;
#endif
#if X64
    public ulong BaseAddress;
    public ulong AllocationBase;
    public uint AllocationProtect;
    public uint __Alignment1;
    public ulong RegionSize;   // size of the region allocated by the program
    public uint State;   // check if allocated (MEM_COMMIT)
    public uint Protect; // page protection (must be PAGE_READWRITE)
    public uint Type;
    public uint __Alignment2;
#endif
}
