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
Test classes have the same name as the classes they test with the suffix "ﾠ<MemberUnderTest>".

Test methods (facts) follow the convention `<Verb>ﾠ<Expectedﾠresultﾠdescription>ﾠwhenﾠ<preconditionﾠdescription>`:

    Extractsﾠtypeﾠwhenﾠtypeﾠisﾠnotﾠnull()

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

### Embedding Whole Libraries or Part of Them

We want to avoid dependecies to third-party libraries in the *Engine*. That's why we embedd thier code into the NSeed assembly and make it internal.
To embedd a whole library or a part of it do the following.

Place the code in a subfolder of the *ThirdParty* folder. The subfolder must have the name of the embedded library.
E.g.

    ThirdParty/CommandLineUtils

Commit that code in a commit with the message "Take over <Library name>".

Adjust the embedded code. Adjustment should be as little as possible, mostly to make the code compileable. Make all the types internal.

Commit the changes in a commit with the message "Adjust <Library Name>". This way we can always track the changes done to the original source code.

Add the appropriate entry to the [third-party licenses file](../licenses/README.md).


### Embedding Code Snippets
In case of embedding a code snippet, "nseedify" it, means change the namespace, style etc.

Put links to the original source code in the following form, together with the name of the original license:

    // Taken and adapted from:
    // <link>

Add the appropriate entry to the [third-party licenses file](../licenses/README.md).

## Extension Classes

Extension classes have the following name "<ExtendedType>Extensions" and are placed in the folder and namespace "Extensions".