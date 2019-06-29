using System;

namespace NSeed.Discovery.Seed
{
    internal interface ISeedTypeExtractor<TSeedImplementation> : IExtractor<TSeedImplementation, Type>
        where TSeedImplementation : class
    {
    }
}