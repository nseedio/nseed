using System.Collections.Generic;
using System.Threading.Tasks;
using SmokeTests.BusinessModel.Entities;
using SmokeTests.Persistence.Repositories;

#if DotNetCore
using Microsoft.EntityFrameworkCore;
#endif

#if DotNetClassic
using System.Data.Entity;
#endif

namespace SmokeTests.BusinessModel.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IUserRepository userRepository;

        public UserService(IUnitOfWork unitOfWork, IUserRepository userRepository)
        {
            this.unitOfWork = unitOfWork;
            this.userRepository = userRepository;
        }

        public async Task CreateAdmin(NewUser admin)
        {
            var newUser = new User
            {
                Username = admin.Username,
                FirstName = admin.FirstName,
                LastName = admin.LastName,
                IsAdmin = true
            };

            userRepository.Users.Add(newUser);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task CreateUser(NewUser user)
        {
            var newUser = new User
            {
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsAdmin = false
            };

            userRepository.Users.Add(newUser);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<IReadOnlyCollection<User>> GetAllUsers()
        {
            return await userRepository.Users.ToListAsync();
        }
    }
}