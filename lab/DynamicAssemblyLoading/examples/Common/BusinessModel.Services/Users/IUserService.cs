using SmokeTests.BusinessModel.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmokeTests.BusinessModel.Services.Users
{
    public interface IUserService
    {
        Task CreateUser(NewUser user);
        Task CreateAdmin(NewUser admin);
        Task<IReadOnlyCollection<User>> GetAllUsers();
    }
}
