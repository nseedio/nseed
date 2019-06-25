using JetBrains.Annotations;
using Light.GuardClauses.Exceptions;
using NSeed;
using NSeed.Extensions;
using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Light.GuardClauses
{
    internal static partial class Check
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static Type MustBeSeedType(this Type parameter, string parameterName)
        {
            if (parameter == null)
                Throw.ArgumentNull(parameterName);

            if (!parameter.IsSeedType())
                Throw.Argument(parameterName);

            return parameter;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsSeedType(this Type parameter)
        {
            return parameter?.GetInterfaces().Contains(typeof(ISeed)) == true;
        }
    }
}

namespace Light.GuardClauses.Exceptions
{
    internal static partial class Throw
    {
        [ContractAnnotation("=> halt")]
        public static void Argument(string parameterName = null, string message = null) => throw new ArgumentException(message ?? $"{parameterName ?? "The value"} is invalid.", parameterName);
    }
}