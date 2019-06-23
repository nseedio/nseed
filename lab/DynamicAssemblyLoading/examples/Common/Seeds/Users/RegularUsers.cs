using NSeed;
using SmokeTests.BusinessModel.Entities;
using SmokeTests.BusinessModel.Services.Users;
using SmokeTests.Persistence.Repositories;
using System;
using System.Threading.Tasks;
using Bogus;

#if DotNetCore
using Microsoft.EntityFrameworkCore;
#endif

#if DotNetClassic
using System.Data.Entity;
#endif

namespace SmokeTests.Seeds.Users
{
    public sealed class RegularUsers : ISeed<User>
    {
        private const int NumberOfRegularUsers = 100;
        private const string UsernameSuffix = "_reg";

        private readonly IUserService userService;
        private readonly IUserRepository userRepository;

        public RegularUsers(IUserService userService, IUserRepository userRepository)
        {
            this.userService = userService;
            this.userRepository = userRepository;
        }

        public async Task Seed()
        {
            Randomizer.Seed = new Random(54321);

            var regularUserFaker = new Faker<NewUser>()                
                .RuleFor(x => x.FirstName, f => f.Name.FirstName())
                .RuleFor(x => x.LastName, f => f.Name.LastName())
                .RuleFor(x => x.Username, (f, user) => f.Internet.UserName(user.FirstName, user.LastName) + UsernameSuffix);

            var newUsers = regularUserFaker.Generate(NumberOfRegularUsers);

            foreach (var newUser in newUsers)
            {
                await userService.CreateUser(new NewUser
                {
                    Username = newUser.Username,
                    FirstName = newUser.FirstName,
                    LastName = newUser.LastName,
                });
            }
        }

        public async Task<bool> OutputAlreadyExists()
        {
            return await userRepository
                .Users
                .CountAsync(user => user.Username.EndsWith(UsernameSuffix)) >= NumberOfRegularUsers;
        }

        public Task UnSeed()
        {
            throw new NotImplementedException();
        }
    }
}