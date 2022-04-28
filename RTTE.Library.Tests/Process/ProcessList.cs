using System;
using NUnit.Framework;
using RTTE.Library.Process;
using LibraryProcessList = RTTE.Library.Process.ProcessList;
using System.Threading.Tasks;
using RTTE.Library.Native;
using LanguageExt;
using NSubstitute;
using System.Collections.Generic;
using System.Linq;
using RTTE.Library.PeFile;
using RTTE.Library.Common.Interfaces;
using System.Text;

namespace RTTE.Library.Tests {
    public class ProcessList {
        [TestCase]
        public void Create_ShouldCreateNewProcessListObject_Always() {
            IWinAPI winAPI = Substitute.For<IWinAPI>();
            IPeFileParser peFileParser = Substitute.For<IPeFileParser>();
            Assert.IsInstanceOf(typeof(LibraryProcessList), LibraryProcessList.Create(winAPI, peFileParser));
        }

        [TestCase("", false, false, ProcessArchitecture.x86)]
        [TestCase("test", false, false, ProcessArchitecture.x86)]
        [TestCase("test", true, false, ProcessArchitecture.x86)]
        [TestCase("", true, false, ProcessArchitecture.x86)]
        public async Task AddDeleteUpdate_ShouldCreateAdvancedProcess_IfMatchesFilterAndIfThereIsANewProcess(
            string filter,
            bool hideInvalid,
            bool hideWindowless,
            ProcessArchitecture currentArchitecture
        ) {
            ProcessFilterOptions filterOptions = new(filter, hideInvalid, hideWindowless, currentArchitecture);

            string processName = "test";
            string expectedProcessName = $"{processName}.exe";

            MockProcess mockProcess = new(new IntPtr(50), 5, processName, IntPtr.Zero, string.Empty);
            IProcess[] processes = new MockProcess[] { mockProcess };

            IWinAPI winAPI = Substitute.For<IWinAPI>();
            _ = winAPI.QueryFullProcessImageName(Arg.Any<IntPtr>(), Arg.Any<int>(), Arg.Any<StringBuilder>(), ref Arg.Any<int>())
                .ReturnsForAnyArgs(callInfo => {
                    _ = callInfo.Arg<StringBuilder>().Append($@"C:\TestFolder\{expectedProcessName}");
                    return true;
                });
            _ = winAPI.PrintWindow(Arg.Any<IntPtr>(), Arg.Any<IntPtr>(), Arg.Any<int>())
                .Returns(true);
            _ = winAPI.OpenProcess(Arg.Any<uint>(), Arg.Any<bool>(), Arg.Any<uint>())
                .Returns(mockProcess.GetHandle().Match(handle => handle, IntPtr.Zero));

            IPeFile peFile = Substitute.For<IPeFile>();
            _ = peFile.GetArchitecture().Returns(ProcessArchitecture.x86);

            IPeFileParser peFileParser = Substitute.For<IPeFileParser>();
            _ = peFileParser.TryParse(Arg.Any<string>())
                .Returns(Option<IPeFile>.Some(peFile));

            IAdvancedProcess[] advancedProcesses = Array.Empty<IAdvancedProcess>();

            async Task<IAdvancedProcess> add(IAdvancedProcess advancedProcess) {
                Assert.IsTrue(advancedProcess.GetName() == expectedProcessName, "Process name is not correct!");
                Assert.IsTrue(advancedProcess.GetId() == mockProcess.GetId(), "Process id is not correct!");
                Assert.IsTrue(advancedProcess.GetHandle() == mockProcess.GetHandle(), "Process handle is not correct!");
                Assert.IsTrue(advancedProcess.GetMainWindowHandle() == mockProcess.GetMainWindowHandle(), "Process Main Window Handle is not correct!");
                Assert.IsTrue(advancedProcess.GetMainWindowTitle() == mockProcess.GetMainWindowTitle(), "Process Main Window Title is not correct!");
                return await Task.FromResult(advancedProcess);
            }

            async Task<IAdvancedProcess> update(IAdvancedProcess advancedProcess, int index) {
                Assert.Fail("This should not update anything!");
                return await Task.FromResult(advancedProcess);
            }

            async Task<IAdvancedProcess> remove(IAdvancedProcess advancedProcess) {
                Assert.Fail("This should not remove anything!");
                return await Task.FromResult(advancedProcess);
            }

            LibraryProcessList processList = LibraryProcessList.Create(winAPI, peFileParser);
            _ = await processList.AddDeleteUpdate(
                processes,
                advancedProcesses,
                filterOptions,
                add,
                update,
                remove
            );
        }

        [TestCase("", false, true, ProcessArchitecture.x86)]
        [TestCase("test", false, true, ProcessArchitecture.x86)]
        [TestCase("teast", false, true, ProcessArchitecture.x86)]
        [TestCase("", true, true, ProcessArchitecture.x86)]
        [TestCase("", false, true, ProcessArchitecture.x64)]
        [TestCase("test4", false, false, ProcessArchitecture.x86)]
        [TestCase("teast", true, false, ProcessArchitecture.x64)]
        [TestCase("", true, false, ProcessArchitecture.x64)]
        public async Task AddDeleteUpdate_ShouldNotCreateAdvancedProcess_IfDoesNotMatchFilter(
            string filter,
            bool hideInvalid,
            bool hideWindowless,
            ProcessArchitecture currentArchitecture
        ) {
            ProcessFilterOptions filterOptions = new(filter, hideInvalid, hideWindowless, currentArchitecture);

            string processName = "test";
            string expectedProcessName = $"{processName}.exe";

            MockProcess mockProcess = new(new IntPtr(50), 5, processName, IntPtr.Zero, string.Empty);
            IProcess[] processes = new MockProcess[] { mockProcess };

            IWinAPI winAPI = Substitute.For<IWinAPI>();
            _ = winAPI.QueryFullProcessImageName(Arg.Any<IntPtr>(), Arg.Any<int>(), Arg.Any<StringBuilder>(), ref Arg.Any<int>())
                .ReturnsForAnyArgs(callInfo => true);
            _ = winAPI.PrintWindow(Arg.Any<IntPtr>(), Arg.Any<IntPtr>(), Arg.Any<int>())
                .Returns(true);
            _ = winAPI.OpenProcess(Arg.Any<uint>(), Arg.Any<bool>(), Arg.Any<uint>())
                .Returns(IntPtr.Zero);

            IPeFile peFile = Substitute.For<IPeFile>();
            _ = peFile.GetArchitecture().Returns(ProcessArchitecture.x86);

            IPeFileParser peFileParser = Substitute.For<IPeFileParser>();
            _ = peFileParser.TryParse(Arg.Any<string>())
                .Returns(Option<IPeFile>.Some(peFile));

            AdvancedProcess[] advancedProcesses = Array.Empty<AdvancedProcess>();

            async Task<IAdvancedProcess> add(IAdvancedProcess advancedProcess) {
                Assert.Fail("This should not add anything!");
                return await Task.FromResult(advancedProcess);
            }

            async Task<IAdvancedProcess> update(IAdvancedProcess advancedProcess, int index) {
                Assert.Fail("This should not update anything!");
                return await Task.FromResult(advancedProcess);
            }

            async Task<IAdvancedProcess> remove(IAdvancedProcess advancedProcess) {
                Assert.Fail("This should not remove anything!");
                return await Task.FromResult(advancedProcess);
            }

            LibraryProcessList processList = LibraryProcessList.Create(winAPI, peFileParser);
            IEnumerable<Option<IAdvancedProcess>> returnedProcesses = await processList.AddDeleteUpdate(
                processes,
                advancedProcesses,
                filterOptions,
                add,
                update,
                remove
            );
            Assert.IsFalse(returnedProcesses.Somes().Any());
        }

        [TestCase("", false, false, ProcessArchitecture.x86)]
        public async Task AddDeleteUpdate_ShouldNotCreateOrUpdateAdvancedProcess_IfAlreadyExistsWithSameInfo(
            string filter,
            bool hideInvalid,
            bool hideWindowless,
            ProcessArchitecture currentArchitecture
        ) {
            ProcessFilterOptions filterOptions = new(filter, hideInvalid, hideWindowless, currentArchitecture);

            string processName = "test";
            string expectedProcessName = $"{processName}.exe";
            string mainWindowTitle = string.Empty;
            int processId = 5;
            IntPtr processHandle = new(50);

            MockProcess mockProcess = new(processHandle, processId, processName, IntPtr.Zero, mainWindowTitle);
            IProcess[] processes = new MockProcess[] { mockProcess };

            IWinAPI winAPI = Substitute.For<IWinAPI>();
            _ = winAPI.QueryFullProcessImageName(Arg.Any<IntPtr>(), Arg.Any<int>(), Arg.Any<StringBuilder>(), ref Arg.Any<int>())
                .Returns(true);
            _ = winAPI.PrintWindow(Arg.Any<IntPtr>(), Arg.Any<IntPtr>(), Arg.Any<int>())
                .Returns(true);
            _ = winAPI.OpenProcess(Arg.Any<uint>(), Arg.Any<bool>(), Arg.Any<uint>())
                .Returns(IntPtr.Zero);

            IPeFile peFile = Substitute.For<IPeFile>();
            _ = peFile.GetArchitecture().Returns(ProcessArchitecture.x86);

            IPeFileParser peFileParser = Substitute.For<IPeFileParser>();
            _ = peFileParser.TryParse(Arg.Any<string>())
                .Returns(Option<IPeFile>.Some(peFile));

            IAdvancedProcess mockAdvanced = Substitute.For<IAdvancedProcess>();
            _ = mockAdvanced.GetId()
                .Returns(processId);
            _ = mockAdvanced.GetHandle()
                .Returns(processHandle);
            _ = mockAdvanced.GetMainWindowTitle()
                .Returns(mainWindowTitle);
            _ = mockAdvanced.GetArchitecture()
                .Returns(ProcessArchitecture.x86);
            IAdvancedProcess[] advancedProcesses = new IAdvancedProcess[] { mockAdvanced };

            async Task<IAdvancedProcess> add(IAdvancedProcess advancedProcess) {
                Assert.Fail("This should not add anything!");
                return await Task.FromResult(advancedProcess);
            }

            async Task<IAdvancedProcess> update(IAdvancedProcess advancedProcess, int index) {
                Assert.Fail("This should not update anything!");
                return await Task.FromResult(advancedProcess);
            }

            async Task<IAdvancedProcess> remove(IAdvancedProcess advancedProcess) {
                Assert.Fail("This should not remove anything!");
                return await Task.FromResult(advancedProcess);
            }

            LibraryProcessList processList = LibraryProcessList.Create(winAPI, peFileParser);
            IEnumerable<Option<IAdvancedProcess>> returnedProcesses = await processList.AddDeleteUpdate(
                processes,
                advancedProcesses,
                filterOptions,
                add,
                update,
                remove
            );
            Assert.IsFalse(returnedProcesses.Somes().Any());
        }

        [TestCase("", false, false, ProcessArchitecture.x86, "new")]
        [TestCase("test", false, false, ProcessArchitecture.x86, "old2")]
        [TestCase("", true, false, ProcessArchitecture.x86, "ol")]
        [TestCase("test", true, false, ProcessArchitecture.x86, "ses")]
        public async Task AddDeleteUpdate_ShouldUpdateExistingProcess_IfInfoHasChangedAndFilterMatches(
            string filter,
            bool hideInvalid,
            bool hideWindowless,
            ProcessArchitecture currentArchitecture,
            string expectedMainWindowTitle
        ) {
            ProcessFilterOptions filterOptions = new(filter, hideInvalid, hideWindowless, currentArchitecture);

            string processName = "test";
            string expectedProcessName = $"{processName}.exe";
            string oldMainWindowTitle = "old";
            int processId = 5;
            IntPtr processHandle = new(50);

            MockProcess mockProcess = new(processHandle, processId, processName, IntPtr.Zero, expectedMainWindowTitle);
            IProcess[] processes = new MockProcess[] { mockProcess };

            IWinAPI winAPI = Substitute.For<IWinAPI>();
            _ = winAPI.QueryFullProcessImageName(Arg.Any<IntPtr>(), Arg.Any<int>(), Arg.Any<StringBuilder>(), ref Arg.Any<int>())
                .ReturnsForAnyArgs(callInfo => {
                    _ = callInfo.Arg<StringBuilder>().Append($@"C:\TestFolder\{expectedProcessName}");
                    return true;
                });
            _ = winAPI.PrintWindow(Arg.Any<IntPtr>(), Arg.Any<IntPtr>(), Arg.Any<int>())
                .Returns(true);
            _ = winAPI.OpenProcess(Arg.Any<uint>(), Arg.Any<bool>(), Arg.Any<uint>())
                .Returns(mockProcess.GetHandle().Match(handle => handle, IntPtr.Zero));

            IPeFile peFile = Substitute.For<IPeFile>();
            _ = peFile.GetArchitecture().Returns(ProcessArchitecture.x86);

            IPeFileParser peFileParser = Substitute.For<IPeFileParser>();
            _ = peFileParser.TryParse(Arg.Any<string>())
                .Returns(Option<IPeFile>.Some(peFile));

            IAdvancedProcess mockAdvanced = Substitute.For<IAdvancedProcess>();
            _ = mockAdvanced.GetId()
                .Returns(processId);
            _ = mockAdvanced.GetName()
                .Returns(expectedProcessName);
            _ = mockAdvanced.GetHandle()
                .Returns(processHandle);
            _ = mockAdvanced.GetMainWindowTitle()
                .Returns(oldMainWindowTitle);
            _ = mockAdvanced.GetArchitecture()
                .Returns(ProcessArchitecture.x86);
            _ = mockAdvanced.GetArchitectureString()
                .Returns(ProcessArchitecture.x86.ToString());
            _ = mockAdvanced.AsString()
                .Returns($"{oldMainWindowTitle}{expectedProcessName}{ProcessArchitecture.x86}{processId}");
            IAdvancedProcess[] advancedProcesses = new IAdvancedProcess[] { mockAdvanced };

            async Task<IAdvancedProcess> add(IAdvancedProcess advancedProcess) {
                Assert.Fail("This should not add anything!");
                return await Task.FromResult(advancedProcess);
            }

            async Task<IAdvancedProcess> update(IAdvancedProcess advancedProcess, int index) {
                Assert.IsTrue(advancedProcess.GetId() == processId);
                Assert.IsTrue(advancedProcess.GetName() == expectedProcessName);
                Assert.IsTrue(advancedProcess.GetMainWindowTitle() == expectedMainWindowTitle);
                return await Task.FromResult(advancedProcess);
            }

            async Task<IAdvancedProcess> remove(IAdvancedProcess advancedProcess) {
                Assert.Fail("This should not remove anything!");
                return await Task.FromResult(advancedProcess);
            }

            LibraryProcessList processList = LibraryProcessList.Create(winAPI, peFileParser);
            _ = await processList.AddDeleteUpdate(
                processes,
                advancedProcesses,
                filterOptions,
                add,
                update,
                remove
            );
        }

        [TestCase("", false, false, ProcessArchitecture.x86)]
        [TestCase("test", false, false, ProcessArchitecture.x86)]
        [TestCase("", true, false, ProcessArchitecture.x86)]
        [TestCase("test", true, false, ProcessArchitecture.x86)]
        public async Task AddDeleteUpdate_ShouldNotUpdateExistingProcess_IfNoInfoHasChangedButAlsoFilterMatches(
            string filter,
            bool hideInvalid,
            bool hideWindowless,
            ProcessArchitecture currentArchitecture
        ) {
            ProcessFilterOptions filterOptions = new(filter, hideInvalid, hideWindowless, currentArchitecture);

            string processName = "test";
            string expectedProcessName = $"{processName}.exe";
            string oldMainWindowTitle = "old";
            int processId = 5;
            IntPtr processHandle = new(50);

            MockProcess mockProcess = new(processHandle, processId, processName, IntPtr.Zero, oldMainWindowTitle);
            IProcess[] processes = new MockProcess[] { mockProcess };

            IWinAPI winAPI = Substitute.For<IWinAPI>();
            _ = winAPI.QueryFullProcessImageName(Arg.Any<IntPtr>(), Arg.Any<int>(), Arg.Any<StringBuilder>(), ref Arg.Any<int>())
                .Returns(true);
            _ = winAPI.PrintWindow(Arg.Any<IntPtr>(), Arg.Any<IntPtr>(), Arg.Any<int>())
                .Returns(true);
            _ = winAPI.OpenProcess(Arg.Any<uint>(), Arg.Any<bool>(), Arg.Any<uint>())
                .Returns(IntPtr.Zero);

            IPeFile peFile = Substitute.For<IPeFile>();
            _ = peFile.GetArchitecture().Returns(ProcessArchitecture.x86);

            IPeFileParser peFileParser = Substitute.For<IPeFileParser>();
            _ = peFileParser.TryParse(Arg.Any<string>())
                .Returns(Option<IPeFile>.Some(peFile));

            IAdvancedProcess mockAdvanced = Substitute.For<IAdvancedProcess>();
            _ = mockAdvanced.GetId()
                .Returns(processId);
            _ = mockAdvanced.GetName()
                .Returns(expectedProcessName);
            _ = mockAdvanced.GetHandle()
                .Returns(processHandle);
            _ = mockAdvanced.GetMainWindowTitle()
                .Returns(oldMainWindowTitle);
            _ = mockAdvanced.GetArchitecture()
                .Returns(ProcessArchitecture.x86);
            _ = mockAdvanced.GetArchitectureString()
                .Returns(ProcessArchitecture.x86.ToString());
            _ = mockAdvanced.AsString()
                .Returns($"{oldMainWindowTitle}{expectedProcessName}{ProcessArchitecture.x86}{processId}");
            IAdvancedProcess[] advancedProcesses = new IAdvancedProcess[] { mockAdvanced };

            async Task<IAdvancedProcess> add(IAdvancedProcess advancedProcess) {
                Assert.Fail("This should not add anything!");
                return await Task.FromResult(advancedProcess);
            }

            async Task<IAdvancedProcess> update(IAdvancedProcess advancedProcess, int index) {
                Assert.Fail("This should not update anything!");
                return await Task.FromResult(advancedProcess);
            }

            async Task<IAdvancedProcess> remove(IAdvancedProcess advancedProcess) {
                Assert.Fail("This should not remove anything!");
                return await Task.FromResult(advancedProcess);
            }

            LibraryProcessList processList = LibraryProcessList.Create(winAPI, peFileParser);
            _ = await processList.AddDeleteUpdate(
                processes,
                advancedProcesses,
                filterOptions,
                add,
                update,
                remove
            );
            Assert.IsTrue(true);
        }

        [TestCase]
        public async Task AddDeleteUpdate_ShouldRemoveProcess_IfDoesNotExistAnymore() {
            ProcessFilterOptions filterOptions = new("", false, false, ProcessArchitecture.x86);

            string processName = "test";
            string expectedProcessName = $"{processName}.exe";
            string oldMainWindowTitle = "old";
            int processId = 5;
            IntPtr processHandle = new(50);

            IProcess[] processes = Array.Empty<IProcess>();

            IWinAPI winAPI = Substitute.For<IWinAPI>();
            IPeFileParser peFileParser = Substitute.For<IPeFileParser>();

            IAdvancedProcess mockAdvanced = Substitute.For<IAdvancedProcess>();
            _ = mockAdvanced.GetId()
                .Returns(processId);
            _ = mockAdvanced.GetName()
                .Returns(expectedProcessName);
            _ = mockAdvanced.GetHandle()
                .Returns(processHandle);
            _ = mockAdvanced.GetMainWindowTitle()
                .Returns(oldMainWindowTitle);
            _ = mockAdvanced.GetArchitecture()
                .Returns(ProcessArchitecture.x86);
            _ = mockAdvanced.GetArchitectureString()
                .Returns(ProcessArchitecture.x86.ToString());
            _ = mockAdvanced.AsString()
                .Returns($"{oldMainWindowTitle}{expectedProcessName}{ProcessArchitecture.x86}{processId}");
            IAdvancedProcess[] advancedProcesses = new IAdvancedProcess[] { mockAdvanced };

            async Task<IAdvancedProcess> add(IAdvancedProcess advancedProcess) {
                Assert.Fail("This should not add anything!");
                return await Task.FromResult(advancedProcess);
            }

            async Task<IAdvancedProcess> update(IAdvancedProcess advancedProcess, int index) {
                Assert.Fail("This should not update anything!");
                return await Task.FromResult(advancedProcess);
            }

            async Task<IAdvancedProcess> remove(IAdvancedProcess advancedProcess) {
                Assert.IsTrue(advancedProcess.AsString() == mockAdvanced.AsString());
                return await Task.FromResult(advancedProcess);
            }

            LibraryProcessList processList = LibraryProcessList.Create(winAPI, peFileParser);
            _ = await processList.AddDeleteUpdate(
                processes,
                advancedProcesses,
                filterOptions,
                add,
                update,
                remove
            );
        }

        [TestCase("", true, false, ProcessArchitecture.x64)]
        [TestCase("test2", false, false, ProcessArchitecture.x86)]
        [TestCase("", false, true, ProcessArchitecture.x86)]
        [TestCase("test", true, true, ProcessArchitecture.x86)]
        [TestCase("test3", true, true, ProcessArchitecture.x86)]
        [TestCase("test3", false, true, ProcessArchitecture.x86)]
        [TestCase("test3", true, true, ProcessArchitecture.x64)]
        public async Task AddDeleteUpdate_ShouldDeleteProcess_IfDoesNotMatchFilter(
            string filter,
            bool hideInvalid,
            bool hideWindowless,
            ProcessArchitecture currentArchitecture
        ) {
            ProcessFilterOptions filterOptions = new(filter, hideInvalid, hideWindowless, currentArchitecture);

            string processName = "test";
            string expectedProcessName = $"{processName}.exe";
            string oldMainWindowTitle = "old";
            int processId = 5;
            IntPtr processHandle = new(50);

            MockProcess mockProcess = new(processHandle, processId, processName, IntPtr.Zero, oldMainWindowTitle);
            IProcess[] processes = new MockProcess[] { mockProcess };

            IWinAPI winAPI = Substitute.For<IWinAPI>();
            _ = winAPI.QueryFullProcessImageName(Arg.Any<IntPtr>(), Arg.Any<int>(), Arg.Any<StringBuilder>(), ref Arg.Any<int>())
                .Returns(true);
            _ = winAPI.PrintWindow(Arg.Any<IntPtr>(), Arg.Any<IntPtr>(), Arg.Any<int>())
                .Returns(true);
            _ = winAPI.OpenProcess(Arg.Any<uint>(), Arg.Any<bool>(), Arg.Any<uint>())
                .Returns(IntPtr.Zero);

            IPeFile peFile = Substitute.For<IPeFile>();
            _ = peFile.GetArchitecture().Returns(ProcessArchitecture.x86);

            IPeFileParser peFileParser = Substitute.For<IPeFileParser>();
            _ = peFileParser.TryParse(Arg.Any<string>())
                .Returns(Option<IPeFile>.Some(peFile));

            IAdvancedProcess mockAdvanced = Substitute.For<IAdvancedProcess>();
            _ = mockAdvanced.GetId()
                .Returns(processId);
            _ = mockAdvanced.GetName()
                .Returns(expectedProcessName);
            _ = mockAdvanced.GetHandle()
                .Returns(processHandle);
            _ = mockAdvanced.GetMainWindowTitle()
                .Returns(oldMainWindowTitle);
            _ = mockAdvanced.GetArchitecture()
                .Returns(ProcessArchitecture.x86);
            _ = mockAdvanced.GetArchitectureString()
                .Returns(ProcessArchitecture.x86.ToString());
            _ = mockAdvanced.AsString()
                .Returns($"{oldMainWindowTitle}{expectedProcessName}{ProcessArchitecture.x86}{processId}");
            IAdvancedProcess[] advancedProcesses = new IAdvancedProcess[] { mockAdvanced };

            async Task<IAdvancedProcess> add(IAdvancedProcess advancedProcess) {
                Assert.Fail("This should not add anything!");
                return await Task.FromResult(advancedProcess);
            }

            async Task<IAdvancedProcess> update(IAdvancedProcess advancedProcess, int index) {
                Assert.Fail("This should not update anything!");
                return await Task.FromResult(advancedProcess);
            }

            async Task<IAdvancedProcess> remove(IAdvancedProcess advancedProcess) {
                Assert.IsTrue(advancedProcess.AsString() == mockAdvanced.AsString());
                return await Task.FromResult(advancedProcess);
            }

            LibraryProcessList processList = LibraryProcessList.Create(winAPI, peFileParser);
            _ = await processList.AddDeleteUpdate(
                processes,
                advancedProcesses,
                filterOptions,
                add,
                update,
                remove
            );
        }
    }
}