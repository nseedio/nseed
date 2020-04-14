using System;

namespace NSeed
{
    // TODO: Error: It can be only on Seeds not on Scenarios. Add this to remarks below.

    /// <summary>
    /// TODO.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class AlwaysRequiredAttribute : Attribute
    {
    }
}
