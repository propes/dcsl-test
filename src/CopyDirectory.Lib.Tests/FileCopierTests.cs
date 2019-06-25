using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace CopyDirectory.Lib.Tests
{
    public class FileCopierTests
    {
        // Requirements:
        // - Write a file copying facility in C# called CopyDirectory
        // - There must be a visual display of each file (including the source file path and file name) being copied during the copying process.
        // - Ideally, the UI must not “block” during the copy process.
        // - Users must be able to pick or specify a source and target directory.
        // - All files and folders (and their contents) must be copied from the source to the target.
        // - You must implement a recursive algorithm to copy the files.

        private const string SourcePath = "TestData/Source";
        private const string TargetPath = "TestData/Target";

        private readonly Mock<ILogger> _loggerMock = new Mock<ILogger>();
        private readonly FileCopier _sut;

        public FileCopierTests()
        {
            _sut = new FileCopier(_loggerMock.Object);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(" ")]
        public async Task CopyDirectory_GivenSourcePathIsNullOrWhitespace_ThrowsException(string sourcePath)
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.CopyDirectoryAsync(sourcePath, TargetPath));
        }

        [Fact]
        public async Task CopyDirectory_GivenSourcePathDoesNotExist_ThrowsException()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.CopyDirectoryAsync("foo", TargetPath));
        }

        [Fact]
        public async Task CopyDirectory_CopiesAllFilesInRootDirectory()
        {
            var expectedFiles = new[] {
                "test1.txt",
                "test2.txt",
                "test3.txt"
            };

            ClearTargetDirectory(TargetPath);

            await _sut.CopyDirectoryAsync(SourcePath, TargetPath);

            var actualFiles = GetFileNamesInDirectory(TargetPath);

            Assert.Equal(expectedFiles, actualFiles);
        }

        [Fact]
        public async Task CopyDirectory_CopiesAllFilesInSubfolder()
        {
            var expectedFiles = new[] {
                "test4.txt",
                "test5.txt"
            };

            ClearTargetDirectory(TargetPath);

            await _sut.CopyDirectoryAsync(SourcePath, TargetPath);

            var actualFiles = GetFileNamesInDirectory(Path.Combine(TargetPath, "Subfolder"));

            Assert.Equal(expectedFiles, actualFiles);
        }

        [Fact]
        public async Task CopyDirectory_LogsAMessageForEachFileCopied()
        {
            ClearTargetDirectory(TargetPath);

            await _sut.CopyDirectoryAsync(SourcePath, TargetPath);

            _loggerMock.Verify(logger => logger.Log(It.IsAny<string>()), Times.Exactly(5));
        }

        private static void ClearTargetDirectory(string targetPath)
        {
            if (Directory.Exists(targetPath))
            {
                Directory.Delete(targetPath, true);
            }
        }

        private string[] GetFileNamesInDirectory(string path)
        {
            return Directory.GetFiles(path)
                .Select(f => Path.GetFileName(f))
                .OrderBy(f => f)
                .ToArray();
        }
    }
}
