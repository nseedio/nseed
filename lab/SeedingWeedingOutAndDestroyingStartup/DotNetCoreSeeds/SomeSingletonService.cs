namespace DotNetCoreSeeds
{
    internal class SomeSingletonService : ISomeSingletonService
    {
        public int GetUniqueValue() => GetHashCode();
    }
}
