// This file isn't generated, but this comment is necessary to exclude it from StyleCop analysis.
// For more info see: https://github.com/DotNetAnalyzers/StyleCopAnalyzers/issues/2108
// <auto-generated/>

// Copyright (c) Nate McMaster.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel;

namespace McMaster.Extensions.CommandLineUtils.Abstractions
{
    /// <summary>
    /// The result of parsing command line arguments.
    /// </summary>
    internal class ParseResult
    {
        /// <summary>
        /// Initializes <see cref="ParseResult"/>.
        /// </summary>
        /// <param name="selectedCommand">The command selected for execution.</param>
        public ParseResult(CommandLineApplication selectedCommand)
        {
            SelectedCommand = selectedCommand ?? throw new ArgumentNullException(nameof(selectedCommand));
        }

#pragma warning disable CS8618 // Non-nullable field is uninitialized.
        /// <summary>
        /// This constructor is obsolete and will be removed in a future version.
        /// The recommended replacement is <see cref="ParseResult(CommandLineApplication)" />
        /// </summary>
        [Obsolete("This constructor is obsolete and will be removed in a future version. The recommended replacement is ctor(CommandLineApplication selectedCommand)")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ParseResult()
        {
        }
#pragma warning restore CS8618 // Non-nullable field is uninitialized.

        /// <summary>
        /// The application or subcommand that matches the command line arguments.
        /// </summary>
        public CommandLineApplication SelectedCommand { get; set; }
    }
}
