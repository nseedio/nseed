using System;

namespace NSeed.MetaInfo
{
    internal interface IErrorCollector
    {
        void Collect(Error error);

        /// <summary>
        /// Returns true if <paramref name="collecting"/> has collected any errors.
        /// </summary>
        bool Collect(Action<IErrorCollector> collecting);
    }
}
