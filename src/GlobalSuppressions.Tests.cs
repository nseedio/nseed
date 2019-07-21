// Supressions used in all xUnit based test projects.
// TODO: Generate this file one day using T4.

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(
    SA1600.Category,
    SA1600.CheckId,
    Justification = SA1600.Justification,
    Scope = SA1600.Scope,
    Target = "NSeed.Tests.Unit")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(
    SA1600.Category,
    SA1600.CheckId,
    Justification = SA1600.Justification,
    Scope = SA1600.Scope,
    Target = "NSeed.Cli.Tests.Unit")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(
    SA1513.Category,
    SA1513.CheckId,
    Justification = SA1513.Justification,
    Scope = SA1513.Scope,
    Target = "NSeed.Tests.Unit")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(
    SA1513.Category,
    SA1513.CheckId,
    Justification = SA1513.Justification,
    Scope = SA1513.Scope,
    Target = "NSeed.Cli.Tests.Unit")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(
    SA1513.Category,
    SA1513.CheckId,
    Justification = SA1513.Justification,
    Scope = SA1513.Scope,
    Target = "NSeed.Tests.Unit")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(
    SA1516.Category,
    SA1516.CheckId,
    Justification = SA1516.Justification,
    Scope = SA1516.Scope,
    Target = "NSeed.Tests.Unit")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(
    SA1516.Category,
    SA1516.CheckId,
    Justification = SA1516.Justification,
    Scope = SA1516.Scope,
    Target = "NSeed.Cli.Tests.Unit")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(
    SA1203.Category,
    SA1203.CheckId,
    Justification = SA1203.Justification,
    Scope = SA1203.Scope,
    Target = "NSeed.Tests.Unit")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(
    SA1203.Category,
    SA1203.CheckId,
    Justification = SA1203.Justification,
    Scope = SA1203.Scope,
    Target = "NSeed.Cli.Tests.Unit")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(
    SA1649.Category,
    SA1649.CheckId,
    Justification = SA1649.Justification,
    Scope = SA1649.Scope,
    Target = "NSeed.Tests.Unit")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(
    SA1649.Category,
    SA1649.CheckId,
    Justification = SA1649.Justification,
    Scope = SA1649.Scope,
    Target = "NSeed.Cli.Tests.Unit")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(
    SA1402.Category,
    SA1402.CheckId,
    Justification = SA1402.Justification,
    Scope = SA1402.Scope,
    Target = "NSeed.Tests.Unit")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(
    SA1402.Category,
    SA1402.CheckId,
    Justification = SA1402.Justification,
    Scope = SA1402.Scope,
    Target = "NSeed.Cli.Tests.Unit")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(
    SA1117.Category,
    SA1117.CheckId,
    Justification = SA1117.Justification,
    Scope = SA1117.Scope,
    Target = "NSeed.Tests.Unit")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(
    SA1117.Category,
    SA1117.CheckId,
    Justification = SA1117.Justification,
    Scope = SA1117.Scope,
    Target = "NSeed.Cli.Tests.Unit")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(
    SA1118.Category,
    SA1118.CheckId,
    Justification = SA1118.Justification,
    Scope = SA1118.Scope,
    Target = "NSeed.Tests.Unit")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(
    SA1118.Category,
    SA1118.CheckId,
    Justification = SA1118.Justification,
    Scope = SA1118.Scope,
    Target = "NSeed.Cli.Tests.Unit")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(
    SA1515.Category,
    SA1515.CheckId,
    Justification = SA1515.Justification,
    Scope = SA1515.Scope,
    Target = "NSeed.Tests.Unit")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(
    SA1515.Category,
    SA1515.CheckId,
    Justification = SA1515.Justification,
    Scope = SA1515.Scope,
    Target = "NSeed.Cli.Tests.Unit")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(
    SA1106.Category,
    SA1106.CheckId,
    Justification = SA1106.Justification,
    Scope = SA1106.Scope,
    Target = "NSeed.Tests.Unit")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(
    SA1106.Category,
    SA1106.CheckId,
    Justification = SA1106.Justification,
    Scope = SA1106.Scope,
    Target = "NSeed.Cli.Tests.Unit")]

#pragma warning disable SA1402 // File may only contain a single type
#pragma warning disable SA1649 // File name should match first type name
internal static class SA1600
{
    public const string Category = "StyleCop.CSharp.DocumentationRules";
    public const string CheckId = "SA1600:Elements should be documented";
    public const string Justification = "xUnit test classes and methods must be public, but we do not want to document them.";
    public const string Scope = "namespaceanddescendants";
}

internal static class SA1513
{
    public const string Category = "StyleCop.CSharp.LayoutRules";
    public const string CheckId = "SA1513:Closing brace should be followed by blank line";
    public const string Justification = "When a test method needs additional types used only in that test method we want to 'glue' them to the method to visually emphasize the intended usage of the types.";
    public const string Scope = "namespaceanddescendants";
}

internal static class SA1516
{
    public const string Category = "StyleCop.CSharp.LayoutRules";
    public const string CheckId = "SA1516:Elements should be separated by blank line";
    public const string Justification = "In test, we often have dummy classes, methods, etc. that we want to write as compact as possible. That increases readability.";
    public const string Scope = "namespaceanddescendants";
}

internal static class SA1203
{
    public const string Category = "StyleCop.CSharp.OrderingRules";
    public const string CheckId = "SA1203:Constants should appear before fields";
    public const string Justification = "In test, we want to keep constants close to the methods in which they are used.";
    public const string Scope = "namespaceanddescendants";
}

internal static class SA1649
{
    public const string Category = "StyleCop.CSharp.DocumentationRules";
    public const string CheckId = "SA1649:File name should match first type name";
    public const string Justification = "In test, we have different conventions for naming the file names and the test classes inside them.";
    public const string Scope = "namespaceanddescendants";
}

internal static class SA1402
{
    public const string Category = "StyleCop.CSharp.MaintainabilityRules";
    public const string CheckId = "SA1402:File may only contain a single type";
    public const string Justification = "In test, we often have many dummy classes used in test methods that are written in the same file.";
    public const string Scope = "namespaceanddescendants";
}

internal static class SA1117
{
    public const string Category = "StyleCop.CSharp.ReadabilityRules";
    public const string CheckId = "SA1117:Parameters should be on same line or separate lines";
    public const string Justification = "In test, sometime we gain better readability when placing parameters in an arbitrary way.";
    public const string Scope = "namespaceanddescendants";
}

internal static class SA1118
{
    public const string Category = "StyleCop.CSharp.ReadabilityRules";
    public const string CheckId = "SA1118:Parameter should not span multiple lines";
    public const string Justification = "In test, sometime we gain better readability when placing parameters in an arbitrary way.";
    public const string Scope = "namespaceanddescendants";
}

internal static class SA1515
{
    public const string Category = "StyleCop.CSharp.LayoutRules";
    public const string CheckId = "SA1515:Single-line comment should be preceded by blank line";
    public const string Justification = "In test, sometime we gain better readability when nesting comments within the lines of code.";
    public const string Scope = "namespaceanddescendants";
}

internal static class SA1106
{
    public const string Category = "StyleCop.CSharp.ReadabilityRules";
    public const string CheckId = "SA1106:Code should not contain empty statements";
    public const string Justification = "In test, we often have dummy test code that is empty like e.g. empty classes.";
    public const string Scope = "namespaceanddescendants";
}
