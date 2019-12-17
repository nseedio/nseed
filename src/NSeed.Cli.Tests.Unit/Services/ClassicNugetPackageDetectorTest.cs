using NSeed.Cli.Abstractions;
using NSeed.Cli.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace NSeed.Cli.Tests.Unit.Services
{
    public class ClassicNugetPackageDetectorTest
    {
        private INugetPackageDetector NugetPackageDetector { get; } = new ClassicNugetPackageDetector();

        public ClassicNugetPackageDetectorTest() { }

        [Fact]
        public void Test()
        {
            // Trebam stvoriti xml file iz stringa pa tako vise puta za sve moguce varijante
            var content = @"<?xml version=""1.0"" encoding=""utf-8""?>
                            <packages>
                              <package id=""Microsoft.Extensions.DependencyInjection"" version=""2.0.0"" targetFramework=""net461"" />
                              <package id=""Microsoft.Extensions.DependencyInjection.Abstractions"" version=""2.0.0"" targetFramework=""net461"" />
                              <package id=""NSeed"" version=""0.1.0"" targetFramework=""net461"" />
                              <package id=""System.ComponentModel.Annotations"" version=""4.6.0"" targetFramework=""net461"" />
                              <package id=""System.Diagnostics.Process"" version=""4.1.0"" targetFramework=""net461"" />
                              <package id=""System.Threading.Thread"" version=""4.0.0"" targetFramework=""net461"" />
                              <package id=""System.ValueTuple"" version=""4.4.0"" targetFramework=""net461"" />
                            </packages>";

            string rootDirectory = @"../../../TestData\";
            Directory.CreateDirectory(rootDirectory);
            File.WriteAllText(Path.Combine(rootDirectory, "packages.config"), content);
        }
    }
}
