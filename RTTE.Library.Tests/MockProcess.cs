using LanguageExt;
using RTTE.Library.Common.Interfaces;
using RTTE.Library.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTTE.Library.Tests {
    public sealed class MockProcess : IProcess {
        private IntPtr Handle { get; }
        private int Id { get; }
        private IntPtr MainWindowHandle { get; }
        private string Name { get; }
        private string MainWindowTitle { get; }

        public MockProcess(IntPtr handle, int id, string name, IntPtr mainWindowHandle, string mainWindowTitle) {
            Handle = handle;
            Id = id;
            Name = name;
            MainWindowHandle = mainWindowHandle;
            MainWindowTitle = mainWindowTitle;
        }

        public Option<IntPtr> GetHandle() => Handle;
        public int GetId() => Id;
        public IntPtr GetMainWindowHandle() => MainWindowHandle;
        public Option<string> GetMainWindowTitle() => MainWindowTitle;
        public string GetName() => Name;
    }
}
