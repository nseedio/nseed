// This file isn't generated, but this comment is necessary to exclude it from StyleCop analysis.
// For more info see: https://github.com/DotNetAnalyzers/StyleCopAnalyzers/issues/2108
// <auto-generated/>

// Copyright (c) Nate McMaster.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace McMaster.Extensions.CommandLineUtils.Conventions
{
    /// <summary>
    /// Builds a collection of conventions.
    /// </summary>
    internal interface IConventionBuilder
    {
        /// <summary>
        /// Add a convention that will be applied later.
        /// </summary>
        /// <param name="convention">The convention</param>
        IConventionBuilder AddConvention(IConvention convention);
    }
}
