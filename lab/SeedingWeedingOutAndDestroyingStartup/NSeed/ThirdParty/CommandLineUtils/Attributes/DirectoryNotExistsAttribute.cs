// This file isn't generated, but this comment is necessary to exclude it from StyleCop analysis.
// For more info see: https://github.com/DotNetAnalyzers/StyleCopAnalyzers/issues/2108
// <auto-generated/>

// Copyright (c) Nate McMaster.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using McMaster.Extensions.CommandLineUtils.Abstractions;
using McMaster.Extensions.CommandLineUtils.Validation;

namespace McMaster.Extensions.CommandLineUtils
{
    /// <summary>
    /// Specifies that the data must not be an already existing directory, not a file.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    internal sealed class DirectoryNotExistsAttribute : FilePathNotExistsAttributeBase
    {
        /// <summary>
        /// Initializes an instance of <see cref="DirectoryNotExistsAttribute"/>.
        /// </summary>
        public DirectoryNotExistsAttribute()
            : base(FilePathType.Directory)
        {
        }
    }
}
