# CliProgressTracker

## Description

*CliProgressTracker* is a custom made progress bar, made to be used in CLI-based *.NET* applications.

## Features

- Utilizing Unicode characters U+2588, U+2589, U+258A, U+258B, U+258C, U+258D, U+258E and U+258F during progress bar building process makes it more smooth during runtime (*Linux*-only feature).
- Ability to measure time-related statistics of tracked process to predict its duration and end time.
- Highly configurable visual appearance.
- Available three visual presets - *Simple*, *Regular*, *Advanced* - to simplify library use when visual customization is not necessary.

## Presents visual appearance

1. *Simple* present:
    <pre> 31%|█████████                     |</pre>
    Presents only current percentage-based progress  of tracked process in both digit-based and visual form (as progress bar).

2. *Regular* preset:
    <pre> Progress:  31%|█████████                     |[47/150]</pre>
    Adds additional '*Progress:*' label at the begging and presents a ratio between number of completed process steps and all steps required to compleat the process.
3. *Advanced* preset:
    <pre> Progress:  31%|█████████                     |[47/150] [13:34|13:36|00:00:01]</pre>
    Additionally presents time-related metrics of tracked process - start time of the process, estimated end time and average time required to compleat one process step.

## Usage

1. Build the *CliProgressTracker* library and add reference to *CliProgressTracker.dll* file to project, in which You want to utilize the progress tracker.
2. Use instance of *ProgressTracker* class similarly to example provided in class doc-string.

It is recommenced to get familiar with doc-strings delivered within a source code.

## Authors

- Jakub Miodunka

## License

This project is licensed under the MIT License - see the *LICENSE.md* file for details
