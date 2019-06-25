using System;

namespace NSeed.Discovery.Seed
{
    internal interface ISeedTypeExtractor<TSeedImplementation>
        where TSeedImplementation : class
    {
        Type ExtractFrom(TSeedImplementation seedImplementation);
    }
}