using System;
using System.Text;

namespace RTTE.Library.Process;

public record ProcessAddressHook(IntPtr ProcessHandle, int PID, Encoding TextEncoding, IntPtr Address);
