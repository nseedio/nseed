using FluentAssertions;
using System;
using System.Reflection;

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

        public static PropertyInfo GetPropertyWithName(this Type type, string propertyName)
        {
            return type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
        }
    }
}
