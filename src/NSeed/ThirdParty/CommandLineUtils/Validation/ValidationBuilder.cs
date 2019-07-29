#nullable enable
// This file isn't generated, but this comment is necessary to exclude it from StyleCop analysis.
// For more info see: https://github.com/DotNetAnalyzers/StyleCopAnalyzers/issues/2108
// <auto-generated/>

// Copyright (c) Nate McMaster.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace McMaster.Extensions.CommandLineUtils.Validation
{
    /// <summary>
    /// Default implementation of <see cref="IOptionValidationBuilder"/> and <see cref="IArgumentValidationBuilder"/>.
    /// </summary>
    internal class ValidationBuilder : IOptionValidationBuilder, IArgumentValidationBuilder
    {
        private readonly CommandArgument? _argument;
        private readonly CommandOption? _option;

        /// <summary>
        /// Creates a new instance of <see cref="ValidationBuilder"/> for a given <see cref="CommandArgument"/>.
        /// </summary>
        /// <param name="argument">The argument.</param>
        public ValidationBuilder(CommandArgument argument)
        {
            _argument = argument ?? throw new ArgumentNullException(nameof(argument));
        }

        /// <summary>
        /// Creates a new instance of <see cref="ValidationBuilder"/> for a given <see cref="CommandOption"/>.
        /// </summary>
        /// <param name="option">The option.</param>
        public ValidationBuilder(CommandOption option)
        {
            _option = option ?? throw new ArgumentNullException(nameof(option));
        }

        /// <summary>
        /// Adds a validator to the argument or option.
        /// </summary>
        /// <param name="validator"></param>
        public void Use(IValidator validator)
        {
            _argument?.Validators.Add(validator);
            _option?.Validators.Add(validator);
        }

        void IArgumentValidationBuilder.Use(IArgumentValidator validator)
        {
            _argument?.Validators.Add(validator);
        }

        void IOptionValidationBuilder.Use(IOptionValidator validator)
        {
            _option?.Validators.Add(validator);
        }
    }
}