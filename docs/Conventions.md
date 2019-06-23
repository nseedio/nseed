# Conventions

## Project Structure Within the `src` Folder

Folder's structure e.g:

    [Templates]
      [DotNetCore]
      [DotNetClassic]
    [Common]
    [NSeed]
      [Seeding]
    [NSeed.Cli]
    NSeed.sln
    NuGet.config

Project names correspond to their folder names:

    NSeed.csproj
    NSeed.Cli.csproj

## Unit Testing

We use [xUnit](https://xunit.net/).
Test project names follow the convention `<ProjectName>.Tests.<TestType>`:

    NSeed.Tests.Unit.csproj
    NSeed.Cli.Tests.Unit.csproj
    NSeed.Cli.Tests.Integration.csproj

Test projects have the same internal folder and namespace structrue as the projects that they test.
Test classes have the same name as the classes they test with the suffix "Tests".

Test methods (facts) follow the convention `<MemberUnderTest>__<Precondition_description>__<Expected_result_description>`:

    Validate__Solution_not_defined_multiple_solutions__Error()

## General Coding Conventions

We do not use underscores to prefix field names.
We have a common .editorconfig across all code repositories.
We do not have commented code.
We have "Warnings as errors" on all projects across all code repositories.