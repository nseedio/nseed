# Changelog
All notable changes to the "NSeed" NuGet package will be documented in this file.

The format of the file is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/).

## [0.2.0] - 2020-03-07
### Added
- `SeedBucket` handles command-line arguments.
- `IScenario` interface.
- `Requires` attribute.
- `Description` attribute.
- `YieldOf` base abstract class.

### Changed
- `ISeed.OutputAlreadyExists()` renamed to `ISeed.HasAlreadyYielded()`.

## [0.1.0] - 2019-06-26
### Added
- `ISeed` interface.
- `FriendlyName` attribute.
- Empty abstract `SeedBucket` base class.