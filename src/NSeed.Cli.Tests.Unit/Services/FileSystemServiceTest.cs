using FluentAssertions;
using NSeed.Cli.Assets;
using NSeed.Cli.Services;
using NSeed.Cli.Subcommands.New.Models;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace NSeed.Cli.Tests.Unit.Services
{
    public class FileFixture : IDisposable
    {
        public const string RootDirectory = @"../../../TestData\";

        private readonly List<(string Directory, string File)> testSlnPaths = new List<(string Directory, string File)>
        {
            ($@"{RootDirectory}Fit\", "Fit.Web.sln"),
            ($@"{RootDirectory}Main\Sub\", "Fit.Web.sln"),
            ($@"{RootDirectory}Test\Sub\", "Fit.Web.sln"),
            ($@"{RootDirectory}Test\Sub\PSubOne\", "FitOne.Web.sln"),
            ($@"{RootDirectory}Pims\", "Pims.sln"),
        };

        public FileFixture()
        {
            foreach (var testPath in testSlnPaths)
            {
                var path = $@"{testPath.Directory}{testPath.File}";
                Directory.CreateDirectory(testPath.Directory);
                if (!string.IsNullOrEmpty(testPath.File))
                {
                    using (File.Create(path))
                    { }
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

    public class FileSystemServiceTest
    {
        public class TryGetSolutionPath : IClassFixture<FileFixture>
        {
            public static IEnumerable<object[]> ValidPaths => new List<object[]>
            {
                new object[] { $@"{FileFixture.RootDirectory}Fit\Fit.Web.sln" },
                new object[] { $@"{FileFixture.RootDirectory}Fit" },
                new object[] { $@"{FileFixture.RootDirectory}Main" },
                new object[] { $@"{FileFixture.RootDirectory}Test\Sub" },
                new object[] { $@"{FileFixture.RootDirectory}\Pims\pims" },
            };

            public static IEnumerable<object[]> InvalidPaths => new List<object[]>
            {
                new object[] { $@"{FileFixture.RootDirectory}Test", Resources.New.Errors.MultipleSolutionsFound },
                new object[] { $@"Test_238e9324-65e0-4f74-ac64-748f5ea32b90", Resources.New.Errors.WorkingDirectoryDoesNotContainAnySolution },
                new object[] { $@"TestXYZ", Resources.New.Errors.WorkingDirectoryDoesNotContainAnySolution },
                new object[] { $@"Test/SubTest/SubSubTest", Resources.New.Errors.SolutionPathDirectoryDoesNotExist },
                new object[] { $@"Test_cd81a251-30d9-40b0-8c98-252c03a720bc.sln", Resources.New.Errors.WorkingDirectoryDoesNotContainAnySolution },
                new object[] { $@"C:/Test238e9324/Test.sln", Resources.New.Errors.SolutionPathDirectoryDoesNotExist },
                new object[] { $@"{FileFixture.RootDirectory}Fit\Fit.Web", Resources.New.Errors.InvalidFile }
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
                var response = service.TryGetSolutionPath(path, out var sln);

                response.IsSuccesful.Should().Be(true);
                sln.Should().NotBeNullOrEmpty();
            }

            [Theory]
#pragma warning disable xUnit1019 // MemberData must reference a member providing a valid data type
            [MemberData(nameof(InvalidPaths))]
#pragma warning restore xUnit1019 // MemberData must reference a member providing a valid data type
            public void IsﾠNotﾠSuccesfulﾠAndﾠReturnsﾠErrorﾠResponse(string path, string errorMessage)
            {
                var service = new FileSystemService();
                var response = service.TryGetSolutionPath(path, out var sln);

                response.IsSuccesful.Should().Be(false);
                response.Message.Should().Be(errorMessage);
                sln.Should().BeNullOrEmpty();
            }
        }

        public class TryGetTemplatePath
        {
            [Fact]
            public void IsﾠSuccesfulﾠAndﾠReturnsﾠValidﾠPath()
            {
                var service = new FileSystemService();
                var response = service.TryGetTemplate(Framework.NETCoreApp, out Template template);

                response.IsSuccesful.Should().BeTrue();
                response.Message.Should().BeNullOrEmpty();
                template.Should().NotBeNull();
                template.Name.Should().NotBeNullOrEmpty();
                Directory.Exists(template.Path).Should().BeTrue();

                service.RemoveTempTemplates();
            }
        }
    }
}
