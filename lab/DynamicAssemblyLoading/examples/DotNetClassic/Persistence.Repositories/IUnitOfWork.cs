using System.Threading.Tasks;

namespace SmokeTests.Persistence.Repositories
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync();
    }
}