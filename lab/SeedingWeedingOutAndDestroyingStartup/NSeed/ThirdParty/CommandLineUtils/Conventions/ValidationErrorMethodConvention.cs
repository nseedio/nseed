// This file isn't generated, but this comment is necessary to exclude it from StyleCop analysis.
// For more info see: https://github.com/DotNetAnalyzers/StyleCopAnalyzers/issues/2108
// <auto-generated/>

// Copyright (c) Nate McMaster.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Reflection;

namespace McMaster.Extensions.CommandLineUtils.Conventions
{
    /// <summary>
    /// Invokes a method named <c>OnValidationError</c> on the model type of <see cref="CommandLineApplication{TModel}"/>
    /// to handle validation errors.
    /// </summary>
    internal class ValidationErrorMethodConvention : IConvention
    {
        /// <inheritdoc />
        public virtual void Apply(ConventionContext context)
        {
            var modelAccessor = context.ModelAccessor;
            if (context.ModelType == null || modelAccessor == null)
            {
                return;
            }

            const BindingFlags MethodFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

            var method = context.ModelType
                .GetTypeInfo()
                .GetMethod("OnValidationError", MethodFlags);

            if (method == null)
            {
                return;
            }

            context.Application.ValidationErrorHandler = (v) =>
            {
                var arguments = ReflectionHelper.BindParameters(method, context.Application, default);
                var result = method.Invoke(modelAccessor.GetModel(), arguments);
                if (method.ReturnType == typeof(int))
                {
                    return (int)result;
                }

                return CommandLineApplication.ValidationErrorExitCode;
            };
        }
    }
}
