using System;
using System.Linq;
using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[CheckBuildProjectConfigurations]
[UnsetVisualStudioEnvironmentVariables]
class Build : NukeBuild
{
    public static int Main () => Execute<Build>(x => x.Compile);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution] readonly Solution Solution;
    [GitRepository] readonly GitRepository GitRepository;
    [GitVersion] readonly GitVersion GitVersion;

    AbsolutePath SourceDirectory => RootDirectory / "src";
    AbsolutePath OutputDirectory => RootDirectory / "output";
    AbsolutePath TemplatesDirectory => RootDirectory / "templates";
    AbsolutePath NSeedCliDirectory => SourceDirectory / "NSeed.Cli";
    AbsolutePath TemplatesZipFile => NSeedCliDirectory / "templates.zip";

    void DeleteOutputFiles()
    {
        OutputDirectory.GlobFiles("*.nupkg", "*.snupkg").ForEach(DeleteFile);
    }

    void CreateTemplatesZip()
    {
        DeleteFile(TemplatesZipFile);
        System.IO.Compression.ZipFile.CreateFromDirectory(TemplatesDirectory, TemplatesZipFile);
    }

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            DeleteOutputFiles();
        });

    Target Restore => _ => _
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetProjectFile(Solution));
        });

    Target CompressTemplates => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            CreateTemplatesZip();
        });

    Target Compile => _ => _
        .DependsOn(CompressTemplates)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .EnableNoRestore()
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration));
        });

    Target Pack => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            DeleteOutputFiles();
            DotNetPack(s => s
                .EnableNoBuild()
                .SetProject(Solution)                
                .SetConfiguration(Configuration)
                .EnableIncludeSymbols()
                .SetSymbolPackageFormat(DotNetSymbolPackageFormat.snupkg)
                .SetOutputDirectory(OutputDirectory));
        });
}