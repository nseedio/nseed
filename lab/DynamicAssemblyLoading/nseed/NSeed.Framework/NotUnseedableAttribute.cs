using System;

namespace NSeed
{
    /// <summary>
    /// Marks a seed class as unseedable. This means that the seed class does not
    /// implement the <see cref="ISeed.UnSeed()"/> method.
    /// </summary>
    /// <remarks>
    /// By default, NSeed assumes that all seeds are unseedable. There are three ways
    /// to tell the framework that a certain seed is not unseedable.
    /// A seed is considered to be unseedable if the <see cref="ISeed.UnSeed()"/>
    /// method throws <see cref="NotImplementedException"/> or <see cref="NotSupportedException"/>
    /// or if the seed class is marked with the <see cref="NotUnseedableAttribute"/>.
    /// The recommended way to mark a seed as unseedable ist to use the <see cref="NotUnseedableAttribute"/>.
    /// That way the discovering and unseeding process knows upfront that a seed is not
    /// unseedable. In the case when exceptions are used, that information is available only
    /// during the unseeding.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class NotUnseedableAttribute : Attribute
    {
    }
}