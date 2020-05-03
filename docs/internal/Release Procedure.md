# Release Procedure

NSeed release procedure consists of the following steps:

- Finalizing release notes
- Testing
- Publishing
- Increasing the version number

Testing must be done by at least two persons on two different machines.

## Finalizing Release Notes

[ ] Finalize release notes
    [ ] CHANGELOG.md
    [ ] NSeed ReleaseNotes.props
    [ ] NSeed CLI ReleaseNotes.props

## Testing

[ ] Run all automated tests on the Debug build
    [ ] On Windows
    [ ] On Linux
    [ ] On Mac
[ ] Create the Release build
[ ] Run all automated tests on the Release build
    [ ] On Windows
    [ ] On Linux
    [ ] On Mac
[ ] Run acceptance tests
    [ ] On Windows
    [ ] On Linux
    [ ] On Mac
[ ] Run examples on NSeed sample project
[ ] Switch the configuration back to Debug

# Publishing

[ ] Publish NuGet packages and debug symbols
[ ] Tag the version in Git
[ ] Create the release on GitHub

# Increasing the Version Number

[ ] Increase the version number
    [ ] CHANGELOG.md (set to [Unreleased])
    [ ] AssemblyInfo.Common.cs
    [ ] BuildProperties.Packages.props
    [ ] Templates
        [ ] nseed_classic_template.csproj
        [ ] nseed_core_template.csproj