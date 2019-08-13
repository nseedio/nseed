using System;

namespace NSeed.Cli.Assets
{
    internal static partial class Resources
    {
        public const string DotNetCoreFramework = ".NETCoreApp";
        public const string DotNetClassicFramework = ".NETFramework";

        // TODO:AM find better way for getting .net and .net core versions
        public static string[] DotNetCoreVersions { get; } = new[]
        {
            "1.0", "1.1", "2.0", "2.1", "2.2", "3.0"
        };

        public static string[] DotNetClassicVersions { get; } = new[]
        {
            "1.0", "1.1", "2.0", "3.0", "3.5", "4", "4.5", "4.5.1", "4.5.2", "4.6", "4.6.1", "4.6.2", "4.7", "4.7.1", "4.7.2", "4.8"
        };

        public static string InitDirectory { get; } = Environment.CurrentDirectory;
    }
}
