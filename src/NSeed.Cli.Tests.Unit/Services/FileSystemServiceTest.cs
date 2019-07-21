using FluentAssertions;
using NSeed.Cli.Services;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using static NSeed.Cli.Resources.Resources;

namespace NSeed.Cli.Tests.Unit.Services
{
    public class FileFixture : IDisposable
    {
        public const string RootDirectory = @"../../../TestData\";

        private readonly List<(string Directory, string File)> TestSlnPaths = new List<(string Directory, string File)>
        {
            ($@"{RootDirectory}Fit\", "Fit.Web.sln"),
            ($@"{RootDirectory}Main\Sub\", "Fit.Web.sln"),
            ($@"{RootDirectory}Test\Sub\", "Fit.Web.sln"),
            ($@"{RootDirectory}Test\Sub\PSubOne\", "FitOne.Web.sln"),
        };

        public FileFixture()
        {
            foreach (var testPath in TestSlnPaths)
            {
                var path = $@"{testPath.Directory}{testPath.File}";
                Directory.CreateDirectory(testPath.Directory);
                if (!string.IsNullOrEmpty(testPath.File))
                {
                    using (File.Create(path)){}
                }
            }
        }

        public void Dispose()
        {
            Directory.Delete(RootDirectory, true);
        }

        protected virtual bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
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
            };

            public static IEnumerable<object[]> InvalidPaths => new List<object[]>
            {
                new object[] { $@"{FileFixture.RootDirectory}Test",Error.MultipleSolutionsFound },
            };

            [Theory]
            [MemberData(nameof(ValidPaths))]
            public void IsﾠSuccesfulﾠAndﾠReturnsﾠValidﾠPath(string path)
            {
                var service = new FileSystemService();
                var response = service.TryGetSolutionPath(path, out var sln);

                response.IsSuccesful.Should().Be(true);
                sln.Should().NotBeNullOrEmpty();
            }

            [Theory]
            [MemberData(nameof(InvalidPaths))]
            public void IsﾠNotﾠSuccesfulﾠAndﾠReturnsﾠErrorﾠResponse(string path, string errorMessage)
            {
                var service = new FileSystemService();
                var response = service.TryGetSolutionPath(path, out var sln);

                response.IsSuccesful.Should().Be(false);
                response.Message.Should().Be(errorMessage);
                sln.Should().BeNullOrEmpty();
            }
        }
    }
}