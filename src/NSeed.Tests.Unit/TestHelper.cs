using FluentAssertions;
using System;

namespace NSeed.Tests.Unit
{
    internal static class TestHelper
    {
        public static Type GetGenericTypeParameter()
        {
            var type = typeof(ClassWithGenericMethod)
                .GetMethod("Method")
                .ReturnType;

            type.Should().Match(it => it.IsGenericParameter);

            return type;
        }

        private class ClassWithGenericMethod
        {
            public T Method<T>(T t) => t;
        }
    }
}
