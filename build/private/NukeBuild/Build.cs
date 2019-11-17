using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using System.IO.Compression;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[CheckBuildProjectConfigurations]
[UnsetVisualStudioEnvironmentVariables]
class Build : NukeBuild
{
    public static int Main() => Execute<Build>(x => x.Compile);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution] readonly Solution solution;
    //[GitRepository] readonly GitRepository gitRepository;
    //[GitVersion] readonly GitVersion gitVersion;

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
        ZipFile.CreateFromDirectory(TemplatesDirectory, TemplatesZipFile, CompressionLevel.Optimal, true);
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
                .SetProjectFile(solution));
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
                .SetProjectFile(solution)
                .SetConfiguration(configuration));
        });

    Target Pack => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            DeleteOutputFiles();
            DotNetPack(s => s
                .EnableNoBuild()
                .SetProject(solution)
                .SetConfiguration(configuration)
                .EnableIncludeSymbols()
                .SetSymbolPackageFormat(DotNetSymbolPackageFormat.snupkg)
                .SetOutputDirectory(OutputDirectory));
        });
}