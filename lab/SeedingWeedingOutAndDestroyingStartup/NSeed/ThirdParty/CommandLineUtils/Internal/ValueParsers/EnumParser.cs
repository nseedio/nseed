// This file isn't generated, but this comment is necessary to exclude it from StyleCop analysis.
// For more info see: https://github.com/DotNetAnalyzers/StyleCopAnalyzers/issues/2108
// <auto-generated/>

// Copyright (c) Nate McMaster.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;

namespace McMaster.Extensions.CommandLineUtils.Abstractions
{
    internal static class EnumParser
    {
        public static IValueParser Create(Type enumType) =>
            ValueParser.Create(enumType, (argName, value, culture) =>
            {
                if (value == null) return Enum.ToObject(enumType, 0);

                try
                {
                    return Enum.Parse(enumType, value, ignoreCase: true);
                }
                catch
                {
                    throw new FormatException(
                        $"Invalid value specified for {argName}. Allowed values are: {string.Join(", ", Enum.GetNames(enumType))}.");
                }
            });
    }
}
