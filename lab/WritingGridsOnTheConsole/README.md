# Writing Grids on the Console

## Description

We want to write out nicely formated grids (tables) on the console. We needs this for different purposes, for example for listing seeds and scenarios.

We are looking for an existing framework that support this.

The mandatory requirements on the framework:

- Must be .NET Standard.
- Must have license that allows to embedd its code into NSeed code. We do not want to have any additional references.
- The embedding of the code should be easy. Ideally the embedded code should be lightweight.
- Must support automatic table and column widths.
- Must support automatic wrapping of long texts.
- Must support automatic truncating of long texts, ideally by adding ellipses (...).
- Must support adding of table column headers.
- Must render well in the Visual Studio Package Manager Console.
- Must work on both Linux and Windows.

Optionally, it would be nice to have:

- Possibility to give different color to each individual cell.

We have found the following frameworks to evaluate:

- [SadConsole](https://github.com/SadConsole/SadConsole)
- [Colorful.Console](https://github.com/tomakita/Colorful.Console)
- [CsConsoleFormat](https://github.com/Athari/CsConsoleFormat)
- [ConsoleTables](https://github.com/khalidabuhakmeh/ConsoleTables)
- [Gui.cs](https://github.com/migueldeicaza/gui.cs)

Interesting articles to read:

- [Using ANSI colour codes in .NET Core Console applications](https://www.jerriepelser.com/blog/using-ansi-color-codes-in-net-console-apps/)

## Running the Experiment

Subfolders contain sample projects. The subfolder name is the same as the framework name.

## Results

### SadConsole

Absolutely awesome, but not what we need. Done for rich interaction and creating games.

### Colorful.Console

Beautiful and colorful, but not what we need. Perfect choice for rich coloring, but has nothing to do with writing grids to the console.

### ConsoleTables

Simple and lightweight, would be a perfect candidate for embedding. It renders nice tables, but unfortunately does not support the needed formating options like word wrap.

### Gui.cs

Another one awesome project, a toolkit with various controls for building rich text user interfaces, but again, not what we need.

### CsConsoleFormat

Incredible project and the absolute winner. The embedding cannot actually be called "lightweight", but it is straightforward.