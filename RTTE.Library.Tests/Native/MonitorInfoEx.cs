using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryMonitorInfoEx = RTTE.Library.Native.MonitorInfoEx;

namespace RTTE.Library.Tests.Native {
    public class MonitorInfoEx {
        [TestCase]
        public void Init_ShouldSetStructValuesCorrectly_Always() {
            LibraryMonitorInfoEx mock = new();
            mock.Init();
            Assert.AreEqual(40 + (2 * LibraryMonitorInfoEx.CCHDEVICENAME), mock.Size, "Size is different!");
            Assert.AreEqual(string.Empty, mock.DeviceName, "DeviceName is different!");
        }
    }
}
