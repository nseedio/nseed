# Changelog
All notable changes to the "NSeed" NuGet package will be documented in this file.

The format of the file is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/).

## [0.3.0-lab3] - Unreleased
### Added
All added features are in the experimental phase and lab-quality:
- `info` command displays seed, scenario, and startup type name below their friendly names.
- `info` command displays yielded entity types without the common namespace. 
- `seed` command supports --filter option.
- `AlwaysRequired` attribute.
- `info` command do not display description columns if there is no at least one description available.

## [0.3.0-lab2] - 2020-04-13 
### Added
All added features are in the experimental phase and lab-quality:
- `ISeedableFilter` interface with concrete implementations: `FullNameEqualsSeedableFilter`, `FullNameContainsSeedableFilter`, `AcceptAllSeedableFilter`.

## [0.3.0-lab1] - 2020-04-11
### Added
All added features are in the experimental phase and lab-quality:
- Seeding of seeds and scenarios.
- `SeedBucketStartup` abstract class.
- `info` command displays errors.

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