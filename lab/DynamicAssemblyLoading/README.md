# Dynamic Assembly Loading

## Description

NSeed engine must discover and execute seeds defined in assemblies. Assemblies can be .NET Core or .NET Classic assemblies. Dynamic loading of assemblies and execution of the code within them is in general a tricky thing to do.

In his article on [.NET Core Plugins](https://natemcmaster.com/blog/2018/07/25/netcore-plugins/) Nate McMaster gave an excellent overview of the major issues when dynamically loading assemblies:

- Locating dependency assemblies
- Dependency mismatch
- Side by side and race conditions
- Native libraries

### Supported Use Cases

The seeds will be executed in these use cases:

- At development time via NSeed CLI tool.
- At development time via Visual Studio Task Runner.
- At development time via IDE plugin (Visual Studio, Visual Studio Code, Rider, VS for Mac, ...).
- In integration tests in popular unit testing frameworks (NUnit, xUnit, ...).
- At deployment time via NSeed CLI tool.

Whichever approach we use to discover and execute seeds, it must work in all of these use cases.

## Running the Experiment

To find a robust and user friendly solution for NSeed we took a look at these .NET tools to see how they approach the problem:

- [xUnit](https://xunit.net/) ([Source](https://github.com/xunit/xunit))
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/) ([Source](https://github.com/aspnet/EntityFrameworkCore))
- [BenchmarkDotNet](https://benchmarkdotnet.org/index.html) ([Source](https://github.com/dotnet/BenchmarkDotNet))
- [Nuke](https://nuke.build/) ([Source](https://github.com/nuke-build/nuke))
- [McMaster.NETCore.Plugins](https://natemcmaster.com/blog/2018/07/25/netcore-plugins/) ([Source](https://github.com/natemcmaster/DotNetCorePlugins))

As a test NSeed engine we took the one used in the first ever public presentation of NSeed held at [ASP.NET Workshop Dublin](https://www.meetup.com/ASP-NET-Workshop-Dublin/events/250802338/).
The engine can be found in the [nseed folder](nseed).

To test the use cases we created to realistic .NET solutions, one for .NET Core and one for .NET Classic.
The solutions can be found in the [examples folder](examples).

We didn't try to create a sample Visual Studio Task Runner or IDE plugin.
Basically, if the seeds can be found and executed via CLI and in unit test runners, we are sure that other use cases will be possible to cover as well.

## Results

Long story short, the technical solution we are going to take is a combination of the approaches found in the following three projects:

- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/) ([Source](https://github.com/aspnet/EntityFrameworkCore))
- [BenchmarkDotNet](https://benchmarkdotnet.org/index.html) ([Source](https://github.com/dotnet/BenchmarkDotNet))
- [Nuke](https://nuke.build/) ([Source](https://github.com/nuke-build/nuke))

The [examples](examples) demonstrate the usage of NSeed based on that technical solution.