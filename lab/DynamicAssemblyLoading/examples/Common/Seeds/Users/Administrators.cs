using NSeed;
using SmokeTests.BusinessModel.Entities;
using SmokeTests.BusinessModel.Services.Users;
using SmokeTests.Persistence.Repositories;
using System;
using System.Threading.Tasks;

#if DotNetCore
using Microsoft.EntityFrameworkCore;
#endif

#if DotNetClassic
using System.Data.Entity;
#endif

namespace SmokeTests.Seeds.Users
{
    public sealed class Administrators : ISeed<User>
    {
        private const string FirstAdminUsername = "admin01";
        private const string SecondAdminUsername = "admin02";

        private readonly IUserService userService;
        private readonly IUserRepository userRepository;

        public Administrators(IUserService userService, IUserRepository userRepository)
        {
            this.userService = userService;
            this.userRepository = userRepository;
        }

        public async Task Seed()
        {
            await userService.CreateAdmin(new NewUser
            {
                Username = FirstAdminUsername,
                FirstName = "Tom",
                LastName = "Johns"
            });

            await userService.CreateAdmin(new NewUser
            {
                Username = SecondAdminUsername,
                FirstName = "Marta",
                LastName = "Weinsberg"
            });
        }

        public async Task<bool> OutputAlreadyExists()
        {
            return await userRepository
                .Users
                .CountAsync(user => user.Username == FirstAdminUsername || user.Username == SecondAdminUsername) >= 2;
        }

        public Task UnSeed()
        {
            throw new NotImplementedException();
        }
    }
}