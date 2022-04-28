using NUnit.Framework;
using RTTE.Library.PeFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryPeFileParser = RTTE.Library.PeFile.PeFileParser;
using PeFileClass = PeNet.PeFile;
using NSubstitute;
using LanguageExt;
using RTTE.Library.File;
using RTTE.Library.Common.Interfaces;

namespace RTTE.Library.Tests.PeFile {
    public class PeFileParser {
        [TestCase]
        public void Create_ShouldCreateNewPeFileParser_Always() {
            static Option<IPeFile> parser(string file)
                => Option<IPeFile>.Some(Substitute.For<IPeFile>());

            IFile mockFile = Substitute.For<IFile>();
            IPeFileParser instance = LibraryPeFileParser.Create(mockFile, parser);
            Assert.IsInstanceOf(typeof(IPeFileParser), instance);
        }

        [TestCase]
        public void TryParse_ShouldReturnSomePeFile_IfFileExistsAndParserIsAbleToParse() {
            static Option<IPeFile> parser(string file)
                => Option<IPeFile>.Some(Substitute.For<IPeFile>());

            IFile mockFile = Substitute.For<IFile>();
            _ = mockFile.Exists("file").Returns(true);
            IPeFileParser instance = LibraryPeFileParser.Create(mockFile, parser);
            Option<IPeFile> result = instance.TryParse("file");
            Assert.IsTrue(result.IsSome);
        }

        [TestCase]
        public void TryParse_ShouldReturnNonePeFile_IfFileDoesNotExist() {
            static Option<IPeFile> parser(string file)
                => Option<IPeFile>.Some(Substitute.For<IPeFile>());

            IFile mockFile = Substitute.For<IFile>();
            _ = mockFile.Exists("file").Returns(false);
            IPeFileParser instance = LibraryPeFileParser.Create(mockFile, parser);
            Option<IPeFile> result = instance.TryParse("file");
            Assert.IsTrue(result.IsNone);
        }

        [TestCase]
        public void TryParse_ShouldReturnNonePeFile_IfFileExistsButParserFails() {
            static Option<IPeFile> parser(string file)
                => Option<IPeFile>.None;

            IFile mockFile = Substitute.For<IFile>();
            _ = mockFile.Exists("file").Returns(true);
            IPeFileParser instance = LibraryPeFileParser.Create(mockFile, parser);
            Option<IPeFile> result = instance.TryParse("file");
            Assert.IsTrue(result.IsNone);
        }
    }
}
