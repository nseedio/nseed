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
We use [Fluent Assertions](https://fluentassertions.com/).
We use [Moq](https://github.com/moq/moq4).

Test project names follow the convention `<ProjectName>.Tests.<TestType>`:

    NSeed.Tests.Unit.csproj
    NSeed.Cli.Tests.Unit.csproj
    NSeed.Cli.Tests.Integration.csproj

Test projects have the same internal folder and namespace structrue as the projects that they test.
Test class file names have the same name as the class file names they test with the suffix ".<MemberUnderTest>.Tests".
Test classes have the same name as the classes they test with the suffix "ﾠ<MemberUnderTest>ﾠTests".

Test methods (facts) follow the convention `Shouldﾠ<Expectedﾠresultﾠdescription>ﾠwhenﾠ<preconditionﾠdescription>`:

    Shouldﾠextractﾠtypeﾠwhenﾠtypeﾠisﾠnotﾠnull()

We use [Hangul the Filler](http://thehumbleprogrammer.com/his-majesty-hangul-the-filler/) as "space": (ﾠ).

For automatic acceptance tests we use BDD approach and write the tests in the *Given-When-Then* form.

## General Coding Conventions

We do not use underscores to prefix field names.
We have a common .editorconfig across all code repositories.
We do not have commented code.
We have "Warnings as errors" on all projects across all code repositories.

## Exception Handling

Exceptions are propagated and handled in general by the top level caller. If applicable some of the middle level callers in the chain can catch them, but only if they know how to handle them.

We throw appropriate Argument Exceptions on all public APIs.
We use `System.Diagnostics.Debug.Assert()` on private and internal members.

We use [Light.GuardClauses](https://github.com/feO2x/Light.GuardClauses).
In the *Engine* we use it not as a NuGet package but as a [single source file embedded in our projects](https://github.com/feO2x/Light.GuardClauses/wiki/Including-Light.GuardClauses-as-source-code).

## Embedding Third-Party Source Code

In case of embedding a single source file, we "nseedify" it, means we change the namespace etc.
To distinguish such code on the file level, the file names will contain the original namespace as prefix.
E.g.

    Light.GuardClauses.Check.cs
    Humanizer.StringHumanizeExtensions.cs

At the top of the file, we put links to the original source code in the following form:

    // Taken and adapted from:
    // <link>

## Extension Classes

Extension classes have the following name "<ExtendedType>Extensions" and are placed in the folder and namespace "Extensions".