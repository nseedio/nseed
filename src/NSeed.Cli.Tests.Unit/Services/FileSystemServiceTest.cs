using FluentAssertions;
using NSeed.Cli.Assets;
using NSeed.Cli.Services;
using NSeed.Cli.Subcommands.New.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace NSeed.Cli.Tests.Unit.Services
{
    /// <summary>
    /// Use RootDirectory and creating all test files and directories that I need in my test's.
    /// This need to be reusable so if I need somewere else also test files use this fixture.
    /// </summary>
    public class FileFixture : IDisposable
    {
        public static readonly string RootDirectory = Path.Combine("..", "..", "..", "TestData");

        private readonly List<(string Directory, string File)> paths =
            new List<(string Directory, string File)>();

        public FileFixture()
        {
            paths = new List<(string Directory, string File)>
            {
                (Path.Combine(RootDirectory, "Fit"), "Fit.Web.sln"),
                (Path.Combine(RootDirectory, "Main", "Sub"), "Fit.Web.sln"),
                (Path.Combine(RootDirectory, "Test", "Sub"), "Fit.Web.sln"),
                (Path.Combine(RootDirectory, "Test", "Sub", "PSubOne"), "FitOne.Web.sln"),
                (Path.Combine(RootDirectory, "Pims"), "Pims.sln"),
                (Path.Combine(RootDirectory, "Projects"), "ProjectDelta.csproj"),
                (Path.Combine(RootDirectory, "EfCore"), "ProjectAlfa.csproj"),
                (Path.Combine(RootDirectory, "EfCore"), "ProjectBeta.csproj"),
            };

            foreach (var path in paths)
            {
                Directory.CreateDirectory(path.Directory);
                if (!string.IsNullOrEmpty(path.File))
                {
                    using (File.Create(Path.Combine(path.Directory, path.File))) { }
                }
            }
        }

        public void Dispose()
        {
            Directory.Delete(RootDirectory, true);
        }

        protected virtual bool IsFileLocked(FileInfo file)
        {
            FileStream? stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                // The file is unavailable because it is:
                // - still being written to
                // - or being processed by another thread
                // - or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            return false;
        }
    }

    public class ValidDataGenerator : IEnumerable<object[]>
    {
        public IEnumerable<object[]> ValidPaths => new List<object[]>
        {
            new object[] { Path.Combine(FileFixture.RootDirectory, "Fit", "Fit.Web.sln") },
            new object[] { Path.Combine(FileFixture.RootDirectory, "Fit") },
            new object[] { Path.Combine(FileFixture.RootDirectory, "Main") },
            new object[] { Path.Combine(FileFixture.RootDirectory, "Test", "Sub") },
            new object[] { Path.Combine(FileFixture.RootDirectory, "Pims", "pims") }
        };

        public IEnumerator<object[]> GetEnumerator() => ValidPaths.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class InvalidDataGenerator : IEnumerable<object[]>
    {
        public IEnumerable<object[]> InvalidPaths => new List<object[]>
        {
            new object[] { Path.Combine(FileFixture.RootDirectory, "Test"), Resources.New.SearchSolutionPathErrors.Instance.MultipleFilesFound },
            new object[] { $@"Test_238e9324-65e0-4f74-ac64-748f5ea32b90", Resources.New.SearchSolutionPathErrors.Instance.WorkingDirectoryDoesNotContainAnyFile },
            new object[] { $@"TestXYZ", Resources.New.SearchSolutionPathErrors.Instance.WorkingDirectoryDoesNotContainAnyFile },
            new object[] { Path.Combine("Test", "SubTest", "SubSubTest"), Resources.New.SearchSolutionPathErrors.Instance.FilePathDirectoryDoesNotExist },
            new object[] { $@"Test_cd81a251-30d9-40b0-8c98-252c03a720bc.sln", Resources.New.SearchSolutionPathErrors.Instance.WorkingDirectoryDoesNotContainAnyFile },
            new object[] { Path.Combine("C:", "Test238e9324", "Test.sln"), Resources.New.SearchSolutionPathErrors.Instance.FilePathDirectoryDoesNotExist },
            new object[] { Path.Combine(FileFixture.RootDirectory, "Fit", "Fit.Web"), Resources.New.SearchSolutionPathErrors.Instance.InvalidFile }
        };

        public IEnumerator<object[]> GetEnumerator() => InvalidPaths.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class FileSystemServiceTest
    {
        [Collection("File Path Collection")]
        public class GetSolutionPath : IClassFixture<FileFixture>
        {
            [Theory]
            // TODO: This looks like a bug in the xUnit analyzer. Strange.
            // For now, just disable it, but take a look at it.
            // It suddenly doesn't work on Igor's machine and we have to see why.
            // Take a look, fix the issue, and remove all disabling of xUnit1019 in all files..
#pragma warning disable xUnit1019 // MemberData must reference a member providing a valid data type
            [ClassData(typeof(ValidDataGenerator))]
#pragma warning restore xUnit1019 // MemberData must reference a member providing a valid data type
            public void IsﾠSuccesfulﾠAndﾠReturnsﾠValidﾠPath(string path)
            {
                var service = new FileSystemService();
                var response = service.GetSolutionPath(path);

                response.IsSuccessful.Should().Be(true);
                response.Payload.Should().NotBeNullOrEmpty();
            }

            [Theory]
#pragma warning disable xUnit1019 // MemberData must reference a member providing a valid data type
            [ClassData(typeof(InvalidDataGenerator))]
#pragma warning restore xUnit1019 // MemberData must reference a member providing a valid data type
            public void IsﾠNotﾠSuccesfulﾠAndﾠReturnsﾠErrorﾠResponse(string path, string errorMessage)
            {
                var service = new FileSystemService();
                var response = service.GetSolutionPath(path);

                response.IsSuccessful.Should().Be(false);
                response.Message.Should().Be(errorMessage);
                response.Payload.Should().BeNullOrEmpty();
            }
        }

        [Collection("File Path Collection")]
        public class GetProjectPath : IClassFixture<FileFixture>
        {
            public static IEnumerable<object[]> ValidPaths => new List<object[]>
            {
                new object[] { Path.Combine(FileFixture.RootDirectory, "Projects") },
                new object[] { Path.Combine(FileFixture.RootDirectory, "Projects", "ProjectDelta") },
                new object[] { Path.Combine(FileFixture.RootDirectory, "Projects", "ProjectDelta.csproj") },
                new object[] { Path.Combine(FileFixture.RootDirectory, "EfCore") }
            };

            public static IEnumerable<object[]> InvalidPaths => new List<object[]>
            {
                new object[] { $@"Test_238e9324-65e0-4f74-ac64-748f5ea32b90", Resources.Info.SearchNSeedProjectPathErrors.Instance.WorkingDirectoryDoesNotContainAnyFile },
                new object[] { $@"TestXYZ", Resources.Info.SearchNSeedProjectPathErrors.Instance.WorkingDirectoryDoesNotContainAnyFile },
                new object[] { Path.Combine("Test", "SubTest", "SubSubTest"), Resources.Info.SearchNSeedProjectPathErrors.Instance.FilePathDirectoryDoesNotExist },
                new object[] { $@"Test_cd81a251-30d9-40b0-8c98-252c03a720bc.csproj", Resources.Info.SearchNSeedProjectPathErrors.Instance.WorkingDirectoryDoesNotContainAnyFile },
                new object[] { Path.Combine("C:", "Test238e9324", "Test.sln"), Resources.Info.SearchNSeedProjectPathErrors.Instance.FilePathDirectoryDoesNotExist },
                new object[] { Path.Combine(FileFixture.RootDirectory, "Fit", "Fit.Web"), Resources.Info.SearchNSeedProjectPathErrors.Instance.InvalidFile }
            };

            [Theory]
            // TODO: This looks like a bug in the xUnit analyzer. Strange.
            // For now, just disable it, but take a look at it.
            // It suddenly doesn't work on Igor's machine and we have to see why.
            // Take a look, fix the issue, and remove all disabling of xUnit1019 in all files..
#pragma warning disable xUnit1019 // MemberData must reference a member providing a valid data type
            [MemberData(nameof(ValidPaths))]
#pragma warning restore xUnit1019 // MemberData must reference a member providing a valid data type
            public void IsﾠSuccesfulﾠAndﾠReturnsﾠValidﾠPath(string path)
            {
                var service = new FileSystemService();
                var response = service.GetNSeedProjectPaths(path);

                response.IsSuccessful.Should().Be(true);
                response.Payload.Should().NotBeNullOrEmpty();
                response.Message.Should().BeEmpty();
            }

            [Theory]
#pragma warning disable xUnit1019 // MemberData must reference a member providing a valid data type
            [MemberData(nameof(InvalidPaths))]
#pragma warning restore xUnit1019 // MemberData must reference a member providing a valid data type
            public void IsﾠNotﾠSuccesfulﾠAndﾠReturnsﾠErrorﾠResponse(string path, string errorMessage)
            {
                var service = new FileSystemService();
                var response = service.GetNSeedProjectPaths(path);

                response.IsSuccessful.Should().Be(false);
                response.Message.Should().Be(errorMessage);
                response.Payload.Should().BeNullOrEmpty();
            }
        }

        public class TryGetTemplatePath
        {
            [Fact]
            public void IsﾠSuccesfulﾠAndﾠReturnsﾠValidﾠPath()
            {
                var service = new FileSystemService();
                var (isSuccesful, message) = service.TryGetTemplate(FrameworkType.NETCoreApp, out Template template);

                isSuccesful.Should().BeTrue();
                message.Should().BeNullOrEmpty();
                template.Should().NotBeNull();
                template.Name.Should().NotBeNullOrEmpty();
                Directory.Exists(template.DirectoryName).Should().BeTrue();

                service.RemoveTempTemplates();
            }
        }
    }
}
