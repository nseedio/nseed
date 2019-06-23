using NSeed.Seeding;
using System.Threading.Tasks;

namespace NSeed.Cli
{
    internal interface ICommandExecutor
    {
        Task Execute<TSeedBucket>() where TSeedBucket : SeedBucket;
    }
}
