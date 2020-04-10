# Changelog
All notable changes to the "NSeed" NuGet package will be documented in this file.

The format of the file is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/).

## [0.3.0-lab1] - 2020-04-11
### Added
All added features are in the experimental phase and lab-quality:
- Seeding of seeds and scenarios.
- `SeedBucketStartup` abstract class.
- The `info` command displays errors.

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