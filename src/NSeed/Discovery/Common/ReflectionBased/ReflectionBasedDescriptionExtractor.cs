﻿using System;
using System.Linq;
using NSeed.MetaInfo;

namespace NSeed.Discovery.Common.ReflectionBased
{
    internal class ReflectionBasedDescriptionExtractor : IDescriptionExtractor<Type>
    {
        string IExtractor<Type, string>.ExtractFrom(Type implementation, IErrorCollector errorCollector)
        {
            System.Diagnostics.Debug.Assert(implementation != null);
            System.Diagnostics.Debug.Assert(errorCollector != null);

            var description = implementation
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .Cast<DescriptionAttribute>()
                .FirstOrDefault()?
                .Description;

            return description ?? string.Empty;
        }
    }
}