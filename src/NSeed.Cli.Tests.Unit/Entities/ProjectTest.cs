using FluentAssertions;
using NSeed.Cli.Abstractions;
using System;
using System.IO;
using Xunit;

namespace NSeed.Cli.Tests.Unit.Entities
{
    public abstract class BaseProjectTest
    {
        private string TestDirectory { get; set; } = string.Empty;

        private string EmptyCoreCsprojFileContent { get; } = string.Empty;
        private string ValidCoreCsprojFileContent { get; } = @"
<Project Sdk=""Microsoft.NET.Sdk"">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>netcoreapp3.0</TargetFramework>
    <AssemblyName>TypicalSeedBucket</AssemblyName>
    <RootNamespace>TypicalSeedBucket</RootNamespace>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include = ""NSeed"" Version=""0.1.0"" />
  </ItemGroup>

</Project>";
        private string InvalidCoreCsprojFileContent { get; } = @"
<Project Sdk=""Microsoft.NET.Sdk"">
    <OutputType>Exe</OutputType>
    <TargetFramework>netcoreapp3.0</TargetFramework>
    <AssemblyName>TypicalSeedBucket</AssemblyName>
    <RootNamespace>TypicalSeedBucket</RootNamespace>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include = ""NSeed"" Version=""0.1.0"" />
  </ItemGroup>

</Project>";
        private string CoreCsprojFileContentWithoutAssemblyName { get; } = @" 
<Project Sdk=""Microsoft.NET.Sdk"">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>netcoreapp3.0</TargetFramework>
    <RootNamespace>TypicalSeedBucket</RootNamespace>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include = ""NSeed"" Version=""0.1.0"" />
  </ItemGroup>

</Project>";

        private string EmptyClassicCsprojFileContent { get; } = string.Empty;
        private string ValidClassicCsprojFileContent { get; } = @"
<Project ToolsVersion=""15.0"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
  <Import Project=""$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props"" Condition=""Exists('$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props')"" />
  <PropertyGroup>
    <Configuration Condition="" '$(Configuration)' == '' "">Debug</Configuration>
    <Platform Condition="" '$(Platform)' == '' "">AnyCPU</Platform>
    <ProjectGuid>{BECF5608-A929-46ED-BA78-5529490E7CE0}</ProjectGuid>
    <OutputType>Exe</OutputType>
    <RootNamespace>TypicalSeedBucket</RootNamespace>
    <AssemblyName>TypicalSeedBucket</AssemblyName>
    <TargetFrameworkVersion>v4.6.1</TargetFrameworkVersion>
    <FileAlignment>512</FileAlignment>
    <AutoGenerateBindingRedirects>true</AutoGenerateBindingRedirects>
    <Deterministic>true</Deterministic>
  </PropertyGroup>
  <PropertyGroup Condition="" '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' "">
    <PlatformTarget>AnyCPU</PlatformTarget>
    <DebugSymbols>true</DebugSymbols>
    <DebugType>full</DebugType>
    <Optimize>false</Optimize>
    <OutputPath>bin\Debug\</OutputPath>
    <DefineConstants>DEBUG;TRACE</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
  </PropertyGroup>
  <PropertyGroup Condition="" '$(Configuration)|$(Platform)' == 'Release|AnyCPU' "">
    <PlatformTarget>AnyCPU</PlatformTarget>
    <DebugType>pdbonly</DebugType>
    <Optimize>true</Optimize>
    <OutputPath>bin\Release\</OutputPath>
    <DefineConstants>TRACE</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
  </PropertyGroup>
  <ItemGroup>
    <Reference Include=""Microsoft.Extensions.DependencyInjection, Version=2.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60, processorArchitecture=MSIL"">
      <HintPath>..\..\..\packages\Microsoft.Extensions.DependencyInjection.2.0.0\lib\netstandard2.0\Microsoft.Extensions.DependencyInjection.dll</HintPath>
    </Reference>
    <Reference Include=""Microsoft.Extensions.DependencyInjection.Abstractions, Version=2.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60, processorArchitecture=MSIL"">
      <HintPath>..\..\..\packages\Microsoft.Extensions.DependencyInjection.Abstractions.2.0.0\lib\netstandard2.0\Microsoft.Extensions.DependencyInjection.Abstractions.dll</HintPath>
    </Reference>
    <Reference Include=""NSeed, Version=0.1.0.0, Culture=neutral, processorArchitecture=MSIL"">
      <HintPath>..\..\..\packages\NSeed.0.1.0\lib\netstandard2.0\NSeed.dll</HintPath>
    </Reference>
    <Reference Include=""System"" />
    <Reference Include=""System.ComponentModel.Annotations, Version=4.2.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a, processorArchitecture=MSIL"">
      <HintPath>..\..\..\packages\System.ComponentModel.Annotations.4.6.0\lib\net461\System.ComponentModel.Annotations.dll</HintPath>
    </Reference>
    <Reference Include=""System.ComponentModel.DataAnnotations"" />
    <Reference Include=""System.Core"" />
    <Reference Include=""System.Diagnostics.Process, Version=4.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a, processorArchitecture=MSIL"">
      <HintPath>..\..\..\packages\System.Diagnostics.Process.4.1.0\lib\net461\System.Diagnostics.Process.dll</HintPath>
      <Private>True</Private>
      <Private>True</Private>
    </Reference>
    <Reference Include=""System.Threading.Thread, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a, processorArchitecture=MSIL"">
      <HintPath>..\..\..\packages\System.Threading.Thread.4.0.0\lib\net46\System.Threading.Thread.dll</HintPath>
      <Private>True</Private>
      <Private>True</Private>
    </Reference>
    <Reference Include=""System.ValueTuple, Version=4.0.2.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51, processorArchitecture=MSIL"">
      <HintPath>..\..\..\packages\System.ValueTuple.4.4.0\lib\net461\System.ValueTuple.dll</HintPath>
      <Private>True</Private>
      <Private>True</Private>
    </Reference>
    <Reference Include=""System.Xml.Linq"" />
    <Reference Include=""System.Data.DataSetExtensions"" />
    <Reference Include=""Microsoft.CSharp"" />
    <Reference Include=""System.Data"" />
    <Reference Include=""System.Net.Http"" />
    <Reference Include=""System.Xml"" />
  </ItemGroup>
  <ItemGroup>
    <Compile Include=""..\TypicalSeedBucket.DotNetCore\Model\Course.cs"">
      <Link>Model\Course.cs</Link>
    </Compile>
    <Compile Include=""..\TypicalSeedBucket.DotNetCore\Model\Person.cs"">
      <Link>Model\Person.cs</Link>
    </Compile>
    <Compile Include=""..\TypicalSeedBucket.DotNetCore\Model\School.cs"">
      <Link>Model\School.cs</Link>
    </Compile>
    <Compile Include=""..\TypicalSeedBucket.DotNetCore\Program.cs"">
      <Link>Program.cs</Link>
    </Compile>
    <Compile Include=""..\TypicalSeedBucket.DotNetCore\Seeding\Courses\EnglishAdvancedCourse.cs"">
      <Link>Seeding\Courses\EnglishAdvancedCourse.cs</Link>
    </Compile>
    <Compile Include=""..\TypicalSeedBucket.DotNetCore\Seeding\Courses\EnglishBeginnersCourse.cs"">
      <Link>Seeding\Courses\EnglishBeginnersCourse.cs</Link>
    </Compile>
    <Compile Include=""..\TypicalSeedBucket.DotNetCore\Seeding\People\EnglishTeachers.cs"">
      <Link>Seeding\People\EnglishTeachers.cs</Link>
    </Compile>
    <Compile Include=""..\TypicalSeedBucket.DotNetCore\Seeding\People\MathTeachers.cs"">
      <Link>Seeding\People\MathTeachers.cs</Link>
    </Compile>
    <Compile Include=""..\TypicalSeedBucket.DotNetCore\Seeding\People\RegularStudents.cs"">
      <Link>Seeding\People\RegularStudents.cs</Link>
    </Compile>
    <Compile Include=""..\TypicalSeedBucket.DotNetCore\Seeding\People\SummerSchoolStudents.cs"">
      <Link>Seeding\People\SummerSchoolStudents.cs</Link>
    </Compile>
    <Compile Include=""..\TypicalSeedBucket.DotNetCore\Seeding\Scenarios\EnglishSummerSchool.cs"">
      <Link>Seeding\Scenarios\EnglishSummerSchool.cs</Link>
    </Compile>
    <Compile Include=""Properties\AssemblyInfo.cs"" />
  </ItemGroup>
  <ItemGroup>
    <None Include=""App.config"" />
    <None Include=""packages.config"" />
  </ItemGroup>
  <ItemGroup />
  <Import Project=""$(MSBuildToolsPath)\Microsoft.CSharp.targets"" />
</Project>
";
        private string InvalidClassicCsprojFileContent { get; } = @"
<Project ToolsVersion=""15.0"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
  <Import Project=""$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props"" Condition=""Exists('$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props')"" />
    <Configuration Condition="" '$(Configuration)' == '' "">Debug</Configuration>
    <Platform Condition="" '$(Platform)' == '' "">AnyCPU</Platform>
    <ProjectGuid>{BECF5608-A929-46ED-BA78-5529490E7CE0}</ProjectGuid>
    <OutputType>Exe</OutputType>
    <RootNamespace>TypicalSeedBucket</RootNamespace>
    <AssemblyName>TypicalSeedBucket</AssemblyName>
    <TargetFrameworkVersion>v4.6.1</TargetFrameworkVersion>
    <FileAlignment>512</FileAlignment>
    <AutoGenerateBindingRedirects>true</AutoGenerateBindingRedirects>
    <Deterministic>true</Deterministic>
  </PropertyGroup>
  <PropertyGroup Condition="" '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' "">
    <PlatformTarget>AnyCPU</PlatformTarget>
    <DebugSymbols>true</DebugSymbols>
    <DebugType>full</DebugType>
    <Optimize>false</Optimize>
    <OutputPath>bin\Debug\</OutputPath>
    <DefineConstants>DEBUG;TRACE</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
  </PropertyGroup>
  <PropertyGroup Condition="" '$(Configuration)|$(Platform)' == 'Release|AnyCPU' "">
    <PlatformTarget>AnyCPU</PlatformTarget>
    <DebugType>pdbonly</DebugType>
    <Optimize>true</Optimize>
    <OutputPath>bin\Release\</OutputPath>
    <DefineConstants>TRACE</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
  </PropertyGroup>
  <ItemGroup>
    <Reference Include=""Microsoft.Extensions.DependencyInjection, Version=2.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60, processorArchitecture=MSIL"">
      <HintPath>..\..\..\packages\Microsoft.Extensions.DependencyInjection.2.0.0\lib\netstandard2.0\Microsoft.Extensions.DependencyInjection.dll</HintPath>
    </Reference>
    <Reference Include=""Microsoft.Extensions.DependencyInjection.Abstractions, Version=2.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60, processorArchitecture=MSIL"">
      <HintPath>..\..\..\packages\Microsoft.Extensions.DependencyInjection.Abstractions.2.0.0\lib\netstandard2.0\Microsoft.Extensions.DependencyInjection.Abstractions.dll</HintPath>
    </Reference>
    <Reference Include=""NSeed, Version=0.1.0.0, Culture=neutral, processorArchitecture=MSIL"">
      <HintPath>..\..\..\packages\NSeed.0.1.0\lib\netstandard2.0\NSeed.dll</HintPath>
    </Reference>
    <Reference Include=""System"" />
    <Reference Include=""System.ComponentModel.Annotations, Version=4.2.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a, processorArchitecture=MSIL"">
      <HintPath>..\..\..\packages\System.ComponentModel.Annotations.4.6.0\lib\net461\System.ComponentModel.Annotations.dll</HintPath>
    </Reference>
    <Reference Include=""System.ComponentModel.DataAnnotations"" />
    <Reference Include=""System.Core"" />
    <Reference Include=""System.Diagnostics.Process, Version=4.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a, processorArchitecture=MSIL"">
      <HintPath>..\..\..\packages\System.Diagnostics.Process.4.1.0\lib\net461\System.Diagnostics.Process.dll</HintPath>
      <Private>True</Private>
      <Private>True</Private>
    </Reference>
    <Reference Include=""System.Threading.Thread, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a, processorArchitecture=MSIL"">
      <HintPath>..\..\..\packages\System.Threading.Thread.4.0.0\lib\net46\System.Threading.Thread.dll</HintPath>
      <Private>True</Private>
      <Private>True</Private>
    </Reference>
    <Reference Include=""System.ValueTuple, Version=4.0.2.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51, processorArchitecture=MSIL"">
      <HintPath>..\..\..\packages\System.ValueTuple.4.4.0\lib\net461\System.ValueTuple.dll</HintPath>
      <Private>True</Private>
      <Private>True</Private>
    </Reference>
    <Reference Include=""System.Xml.Linq"" />
    <Reference Include=""System.Data.DataSetExtensions"" />
    <Reference Include=""Microsoft.CSharp"" />
    <Reference Include=""System.Data"" />
    <Reference Include=""System.Net.Http"" />
    <Reference Include=""System.Xml"" />
  </ItemGroup>
  <ItemGroup>
    <Compile Include=""..\TypicalSeedBucket.DotNetCore\Model\Course.cs"">
      <Link>Model\Course.cs</Link>
    </Compile>
    <Compile Include=""..\TypicalSeedBucket.DotNetCore\Model\Person.cs"">
      <Link>Model\Person.cs</Link>
    </Compile>
    <Compile Include=""..\TypicalSeedBucket.DotNetCore\Model\School.cs"">
      <Link>Model\School.cs</Link>
    </Compile>
    <Compile Include=""..\TypicalSeedBucket.DotNetCore\Program.cs"">
      <Link>Program.cs</Link>
    </Compile>
    <Compile Include=""..\TypicalSeedBucket.DotNetCore\Seeding\Courses\EnglishAdvancedCourse.cs"">
      <Link>Seeding\Courses\EnglishAdvancedCourse.cs</Link>
    </Compile>
    <Compile Include=""..\TypicalSeedBucket.DotNetCore\Seeding\Courses\EnglishBeginnersCourse.cs"">
      <Link>Seeding\Courses\EnglishBeginnersCourse.cs</Link>
    </Compile>
    <Compile Include=""..\TypicalSeedBucket.DotNetCore\Seeding\People\EnglishTeachers.cs"">
      <Link>Seeding\People\EnglishTeachers.cs</Link>
    </Compile>
    <Compile Include=""..\TypicalSeedBucket.DotNetCore\Seeding\People\MathTeachers.cs"">
      <Link>Seeding\People\MathTeachers.cs</Link>
    </Compile>
    <Compile Include=""..\TypicalSeedBucket.DotNetCore\Seeding\People\RegularStudents.cs"">
      <Link>Seeding\People\RegularStudents.cs</Link>
    </Compile>
    <Compile Include=""..\TypicalSeedBucket.DotNetCore\Seeding\People\SummerSchoolStudents.cs"">
      <Link>Seeding\People\SummerSchoolStudents.cs</Link>
    </Compile>
    <Compile Include=""..\TypicalSeedBucket.DotNetCore\Seeding\Scenarios\EnglishSummerSchool.cs"">
      <Link>Seeding\Scenarios\EnglishSummerSchool.cs</Link>
    </Compile>
    <Compile Include=""Properties\AssemblyInfo.cs"" />
  </ItemGroup>
  <ItemGroup>
    <None Include=""App.config"" />
    <None Include=""packages.config"" />
  </ItemGroup>
  <ItemGroup />
  <Import Project=""$(MSBuildToolsPath)\Microsoft.CSharp.targets"" />
</Project>
";
        private string ClassicFileContentWithoutAssemblyName { get; } = @"
<Project ToolsVersion=""15.0"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
  <Import Project=""$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props"" Condition=""Exists('$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props')"" />
  <PropertyGroup>
    <Configuration Condition="" '$(Configuration)' == '' "">Debug</Configuration>
    <Platform Condition="" '$(Platform)' == '' "">AnyCPU</Platform>
    <ProjectGuid>{BECF5608-A929-46ED-BA78-5529490E7CE0}</ProjectGuid>
    <OutputType>Exe</OutputType>
    <RootNamespace>TypicalSeedBucket</RootNamespace>
    <TargetFrameworkVersion>v4.6.1</TargetFrameworkVersion>
    <FileAlignment>512</FileAlignment>
    <AutoGenerateBindingRedirects>true</AutoGenerateBindingRedirects>
    <Deterministic>true</Deterministic>
  </PropertyGroup>
  <PropertyGroup Condition="" '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' "">
    <PlatformTarget>AnyCPU</PlatformTarget>
    <DebugSymbols>true</DebugSymbols>
    <DebugType>full</DebugType>
    <Optimize>false</Optimize>
    <OutputPath>bin\Debug\</OutputPath>
    <DefineConstants>DEBUG;TRACE</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
  </PropertyGroup>
  <PropertyGroup Condition="" '$(Configuration)|$(Platform)' == 'Release|AnyCPU' "">
    <PlatformTarget>AnyCPU</PlatformTarget>
    <DebugType>pdbonly</DebugType>
    <Optimize>true</Optimize>
    <OutputPath>bin\Release\</OutputPath>
    <DefineConstants>TRACE</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
  </PropertyGroup>
  <ItemGroup>
    <Reference Include=""Microsoft.Extensions.DependencyInjection, Version=2.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60, processorArchitecture=MSIL"">
      <HintPath>..\..\..\packages\Microsoft.Extensions.DependencyInjection.2.0.0\lib\netstandard2.0\Microsoft.Extensions.DependencyInjection.dll</HintPath>
    </Reference>
    <Reference Include=""Microsoft.Extensions.DependencyInjection.Abstractions, Version=2.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60, processorArchitecture=MSIL"">
      <HintPath>..\..\..\packages\Microsoft.Extensions.DependencyInjection.Abstractions.2.0.0\lib\netstandard2.0\Microsoft.Extensions.DependencyInjection.Abstractions.dll</HintPath>
    </Reference>
    <Reference Include=""NSeed, Version=0.1.0.0, Culture=neutral, processorArchitecture=MSIL"">
      <HintPath>..\..\..\packages\NSeed.0.1.0\lib\netstandard2.0\NSeed.dll</HintPath>
    </Reference>
    <Reference Include=""System"" />
    <Reference Include=""System.ComponentModel.Annotations, Version=4.2.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a, processorArchitecture=MSIL"">
      <HintPath>..\..\..\packages\System.ComponentModel.Annotations.4.6.0\lib\net461\System.ComponentModel.Annotations.dll</HintPath>
    </Reference>
    <Reference Include=""System.ComponentModel.DataAnnotations"" />
    <Reference Include=""System.Core"" />
    <Reference Include=""System.Diagnostics.Process, Version=4.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a, processorArchitecture=MSIL"">
      <HintPath>..\..\..\packages\System.Diagnostics.Process.4.1.0\lib\net461\System.Diagnostics.Process.dll</HintPath>
      <Private>True</Private>
      <Private>True</Private>
    </Reference>
    <Reference Include=""System.Threading.Thread, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a, processorArchitecture=MSIL"">
      <HintPath>..\..\..\packages\System.Threading.Thread.4.0.0\lib\net46\System.Threading.Thread.dll</HintPath>
      <Private>True</Private>
      <Private>True</Private>
    </Reference>
    <Reference Include=""System.ValueTuple, Version=4.0.2.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51, processorArchitecture=MSIL"">
      <HintPath>..\..\..\packages\System.ValueTuple.4.4.0\lib\net461\System.ValueTuple.dll</HintPath>
      <Private>True</Private>
      <Private>True</Private>
    </Reference>
    <Reference Include=""System.Xml.Linq"" />
    <Reference Include=""System.Data.DataSetExtensions"" />
    <Reference Include=""Microsoft.CSharp"" />
    <Reference Include=""System.Data"" />
    <Reference Include=""System.Net.Http"" />
    <Reference Include=""System.Xml"" />
  </ItemGroup>
  <ItemGroup>
    <Compile Include=""..\TypicalSeedBucket.DotNetCore\Model\Course.cs"">
      <Link>Model\Course.cs</Link>
    </Compile>
    <Compile Include=""..\TypicalSeedBucket.DotNetCore\Model\Person.cs"">
      <Link>Model\Person.cs</Link>
    </Compile>
    <Compile Include=""..\TypicalSeedBucket.DotNetCore\Model\School.cs"">
      <Link>Model\School.cs</Link>
    </Compile>
    <Compile Include=""..\TypicalSeedBucket.DotNetCore\Program.cs"">
      <Link>Program.cs</Link>
    </Compile>
    <Compile Include=""..\TypicalSeedBucket.DotNetCore\Seeding\Courses\EnglishAdvancedCourse.cs"">
      <Link>Seeding\Courses\EnglishAdvancedCourse.cs</Link>
    </Compile>
    <Compile Include=""..\TypicalSeedBucket.DotNetCore\Seeding\Courses\EnglishBeginnersCourse.cs"">
      <Link>Seeding\Courses\EnglishBeginnersCourse.cs</Link>
    </Compile>
    <Compile Include=""..\TypicalSeedBucket.DotNetCore\Seeding\People\EnglishTeachers.cs"">
      <Link>Seeding\People\EnglishTeachers.cs</Link>
    </Compile>
    <Compile Include=""..\TypicalSeedBucket.DotNetCore\Seeding\People\MathTeachers.cs"">
      <Link>Seeding\People\MathTeachers.cs</Link>
    </Compile>
    <Compile Include=""..\TypicalSeedBucket.DotNetCore\Seeding\People\RegularStudents.cs"">
      <Link>Seeding\People\RegularStudents.cs</Link>
    </Compile>
    <Compile Include=""..\TypicalSeedBucket.DotNetCore\Seeding\People\SummerSchoolStudents.cs"">
      <Link>Seeding\People\SummerSchoolStudents.cs</Link>
    </Compile>
    <Compile Include=""..\TypicalSeedBucket.DotNetCore\Seeding\Scenarios\EnglishSummerSchool.cs"">
      <Link>Seeding\Scenarios\EnglishSummerSchool.cs</Link>
    </Compile>
    <Compile Include=""Properties\AssemblyInfo.cs"" />
  </ItemGroup>
  <ItemGroup>
    <None Include=""App.config"" />
    <None Include=""packages.config"" />
  </ItemGroup>
  <ItemGroup />
  <Import Project=""$(MSBuildToolsPath)\Microsoft.CSharp.targets"" />
</Project>
";

        protected string CreateCsprojFile(string name, string content)
        {
            // TODO: am Please use FileFixture here.
            TestDirectory = Path.Combine("..", "..", "..", $"TestData_{Guid.NewGuid()}");
            var path = Path.Combine(TestDirectory, $"{name}.csproj");
            Directory.CreateDirectory(TestDirectory);
            using var tw = new StreamWriter(path, true);
            tw.WriteLine(content);
            return path;
        }

        protected void DeleteTestData()
        {
            Directory.Delete(TestDirectory, true);
        }

        public class GetCoreProjectName : BaseProjectTest
        {
            [Fact]
            public void WhenCsprojFileIsEmpty()
            {
                var project = new Project(
                    CreateCsprojFile("EmptyCoreCsprojFile", EmptyCoreCsprojFileContent),
                    new Framework(Assets.FrameworkType.NETCoreApp, "2.0"));

                project.Name.Should().BeEmpty();
                project.ErrorMessage.Should().BeEquivalentTo(Assets.Resources.Info.Errors.SeedBucketProjectNameCouldNotBeDefined);

                DeleteTestData();
            }

            [Fact]
            public void WhenCsprojFileIsValid()
            {
                var project = new Project(
                   CreateCsprojFile("ValidCoreCsprojFile", ValidCoreCsprojFileContent),
                   new Framework(Assets.FrameworkType.NETCoreApp, "2.0"));

                project.Name.Should().Be("TypicalSeedBucket");

                DeleteTestData();
            }

            [Fact]
            public void WhenCsprojFileIsInvalid()
            {
                var project = new Project(
                    CreateCsprojFile("InvalidCoreCsprojFile", InvalidCoreCsprojFileContent),
                    new Framework(Assets.FrameworkType.NETCoreApp, "2.0"));

                project.Name.Should().BeEmpty();
                project.ErrorMessage.Should().BeEquivalentTo(Assets.Resources.Info.Errors.SeedBucketProjectNameCouldNotBeDefined);

                DeleteTestData();
            }

            [Fact]
            public void WhenCsprojFileIsMissingAssemblyName()
            {
                var project = new Project(
                    CreateCsprojFile("CoreCsprojFileWithoutAssemblyName", CoreCsprojFileContentWithoutAssemblyName),
                    new Framework(Assets.FrameworkType.NETCoreApp, "2.0"));

                project.Name.Should().BeEmpty();

                DeleteTestData();
            }
        }

        public class GetClassicProjectName : BaseProjectTest
        {
            [Fact]
            public void WhenCsprojFileIsEmpty()
            {
                var project = new Project(
                    CreateCsprojFile("EmptyClassicCsprojFile", EmptyClassicCsprojFileContent),
                    new Framework(Assets.FrameworkType.NETFramework, "2.0"));

                project.Name.Should().BeEmpty();
                project.ErrorMessage.Should().BeEquivalentTo(Assets.Resources.Info.Errors.SeedBucketProjectNameCouldNotBeDefined);

                DeleteTestData();
            }

            [Fact]
            public void WhenCsprojFileIsValid()
            {
                var project = new Project(
                    CreateCsprojFile("ValidClassicCsprojFile", ValidClassicCsprojFileContent),
                    new Framework(Assets.FrameworkType.NETFramework, "2.0"));

                project.Name.Should().Be("TypicalSeedBucket");

                DeleteTestData();
            }

            [Fact]
            public void WhenCsprojFileIsInvalid()
            {
                var project = new Project(
                    CreateCsprojFile("InvalidClassicCsprojFile", InvalidClassicCsprojFileContent),
                    new Framework(Assets.FrameworkType.NETFramework, "2.0"));

                project.Name.Should().BeEmpty();
                project.ErrorMessage.Should().BeEquivalentTo(Assets.Resources.Info.Errors.SeedBucketProjectNameCouldNotBeDefined);

                DeleteTestData();
            }

            [Fact]
            public void WhenCsprojFileIsMissingAssemblyName()
            {
                var project = new Project(
                    CreateCsprojFile("ClassicCsprojFileWithoutAssemblyName", ClassicFileContentWithoutAssemblyName),
                    new Framework(Assets.FrameworkType.NETFramework, "2.0"));

                project.Name.Should().BeEmpty();

                DeleteTestData();
            }
        }
    }
}
