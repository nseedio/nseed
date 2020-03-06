namespace NSeed.Discovery
{
    internal interface IDiscoverer<TScope, TDiscovery>
    {
        Discovery<TDiscovery> DiscoverIn(TScope source);
    }
}
