using System;

namespace RTTE.Library.Native;

public struct SystemInfo {
    public ushort ProcessorArchitecture;
    public ushort Reserved;
    public uint PageSize;
    public IntPtr MinimumApplicationAddress;  // minimum address
    public IntPtr MaximumApplicationAddress;  // maximum address
    public IntPtr ActiveProcessorMask;
    public uint NumberOfProcessors;
    public uint ProcessorType;
    public uint AllocationGranularity;
    public ushort ProcessorLevel;
    public ushort ProcessorRevision;
}
