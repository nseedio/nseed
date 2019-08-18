# Parsing Command Line Arguments Without Command Execution

## Description

To create [`ConsoleOutputSink`](../../src/Common/Cli/ConsoleOutputSink.cs) we need the information about verbosity and no-color.
This information is passed over the command line through the `-v|--verbose` and `-nc|--no-color` options.

The `ConsoleOutputSink` is used as the singleton implementation of the `IOutputSink`.
This means that it has to be created and registered as a service before we `ExecuteAsync(commandLineArguments)`.

This then means that we have to parse the command line parameters in order to extract `--verbose` and `--no-color`, but without executing the commands.

In this lab, we try to get this working by simulating the command model which we will at the end have in the CLIs.

## Running the Experiment

Run [RunExperiment.bat](RunExperiment.bat).

## Results

The [Program.cs](Experiment/Program.cs) shows how to safely parse the command line without executing the commands and how to check if the verbosity or no-color is set.