using NSeed.Cli.Assets;
using System.Collections.Generic;

namespace NSeed.Cli.Abstractions
{
    internal interface IFramework
    {
        public FrameworkType Type { get; }

        public string Version { get; }

        public string Name { get; }

        public bool IsDefined { get; }

        public IEnumerable<string> Dependencies { get; }
    }
}
