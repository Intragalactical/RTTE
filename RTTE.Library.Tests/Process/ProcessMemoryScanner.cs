using LanguageExt;
using NSubstitute;
using NUnit.Framework;
using RTTE.Library.Common.Interfaces;
using RTTE.Library.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryProcessMemoryScanner = RTTE.Library.Process.ProcessMemoryScanner;

namespace RTTE.Library.Tests.Process {
    public class ProcessMemoryScanner {
        [TestCase(ProcessArchitecture.x64)]
        [TestCase(ProcessArchitecture.x86)]
        public void Create_ShouldCreateNewProcessMemoryScanner_IfArchitectureIsValid(ProcessArchitecture architecture) {
            Option<ProcessArchitecture> optionedArchitecture = Option<ProcessArchitecture>.Some(architecture);

            IWinAPI mockAPI = Substitute.For<IWinAPI>();
            IAdvancedProcess mockAdvanced = Substitute.For<IAdvancedProcess>();
            _ = mockAdvanced.GetArchitecture()
                .Returns(optionedArchitecture);
            _ = mockAdvanced.GetId()
                .Returns(5);
            _ = mockAdvanced.GetName()
                .Returns("test.exe");
            _ = mockAdvanced.GetHandle()
                .Returns(new IntPtr(50));
            _ = mockAdvanced.GetMainWindowTitle()
                .Returns("main window");
            Option<LibraryProcessMemoryScanner> instance = LibraryProcessMemoryScanner.Create(mockAPI, mockAdvanced);
            _ = instance.Match(instance => Assert.Pass(), () => Assert.Fail());
        }

        [TestCase]
        public void Create_ShouldCreateNone_IfArchitectureIsInvalid() {
            Option<ProcessArchitecture> optionedArchitecture = Option<ProcessArchitecture>.None;

            IWinAPI mockAPI = Substitute.For<IWinAPI>();
            IAdvancedProcess mockAdvanced = Substitute.For<IAdvancedProcess>();
            _ = mockAdvanced.GetArchitecture()
                .Returns(optionedArchitecture);
            _ = mockAdvanced.GetId()
                .Returns(5);
            _ = mockAdvanced.GetName()
                .Returns("test.exe");
            _ = mockAdvanced.GetHandle()
                .Returns(new IntPtr(50));
            _ = mockAdvanced.GetMainWindowTitle()
                .Returns("main window");
            Option<LibraryProcessMemoryScanner> instance = LibraryProcessMemoryScanner.Create(mockAPI, mockAdvanced);
            _ = instance.Match(instance => Assert.Fail(), () => Assert.Pass());
        }
    }
}
