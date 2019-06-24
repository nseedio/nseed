# Source Control

## Repositories

Naming convention is [kebab case](https://medium.com/swlh/string-case-styles-camel-pascal-snake-and-kebab-case-981407998841). E.g: some-repository.

Repositories:

- nseed: *Engine*, *Tool*
- nseed-visual-studio-task-runner
- nseed-examples
- nseed.io: Website

## Repository Structure

For code repositories:

    [benchmarks]
      [FirstBenchmark]
      [SecondBenchmark]
    [build]
      [private]
        [NukeBuild]
    [docs]
    [images]
    [lab]
      [FirstExperiment]
      [SecondExperiment]
    [src]
    [tests]
      [acceptance]
      [integration]
      [smoke]
        [FirstConcreteSmokeTest]
        [SecondConcreteSmokeTest]
      [stress]

Each folder has *README.md*.

## Commits

We use [Commitizen](https://dev.bleacherreport.com/how-we-use-commitizen-to-clean-up-commit-messages-a16790dcd2fd).
We do not have components.

We use two additional types:

- *lab* - For lab experiments.
- *perf* - For performance improvements.

We have linear history (rebase before merge).