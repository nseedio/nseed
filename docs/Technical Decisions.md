# Technical Decisions

## Terminology

*Seed Bucket* - Project that contains seeds, written by the end user, and created with `nseed new`.
*Tool* - NSeed command line tool (CLI).
*Engine* - NSeed abstractions, seeding, and command line argument parsing.
*Core Engine* - Only NSeed abstractions and seeding, without command line parsing.
*Parser* - Command line parser.

## Overall Architecture

The technical solution is a combination of the approaches found in the following three projects:

- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/) ([Source](https://github.com/aspnet/EntityFrameworkCore))
- [BenchmarkDotNet](https://benchmarkdotnet.org/index.html) ([Source](https://github.com/dotnet/BenchmarkDotNet))
- [Nuke](https://nuke.build/) ([Source](https://github.com/nuke-build/nuke))

For the reason behind this technical choice see the lab experiment ["Dynamic Assembly Loading"](https://github.com/nseedio/nseed/tree/master/lab/DynamicAssemblyLoading).

Every *Seed Bucket* is a self-contained executable with the following `Main()` method:

    class Seeds : SeedBucket
    {
        static async Task Main(string[] args) => await Seed<Seeds>(args);
    }

The actually interface is a command line interface (`args`).

## Command Line Parsing

We use [CommandLineUtils](https://github.com/natemcmaster/CommandLineUtils) as the *Parser*.

In the *Engine* we copy the *Parser* code locally to avoid leakage of technical dependecies.
In the *Tool* we reference the *Parser*.

Parsing interface and the validation is reused between the *Tool* and the *Engine*.
It is a code (and not binary) reuse via [Shared projects](https://dev.to/rionmonster/sharing-is-caring-using-shared-projects-in-aspnet-e17).

## Usage

For developing seeds, there is only one NuGet package called `NSeed` with two namespaces, `NSeed` and `NSeed.Seeding`.
For regular scenarios end users need only abstractions defined in the `NSeed` namespace.
For advanced scenarios (e.g. full control over seeding in integration tests) they can use `NSeed.Seeding`.