using NSeed;
using System;
using System.Threading.Tasks;

namespace SeedBucketWithExceptionInConstructor
{
    internal class SeedBucketWithExceptionInConstructor : SeedBucket
    {
        public SeedBucketWithExceptionInConstructor()
        {
            throw new Exception("This is some simulated exception.");
        }

        internal static async Task<int> Main(string[] args)
            => await Handle<SeedBucketWithExceptionInConstructor>(args);
    }
}
