using NUnit.Framework;
using SmokeTests.BusinessModel.Services.Users;
using SmokeTests.Seeds.Users;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessModel.Services.Tests.Unit.Users
{
    [TestFixture]
    public class UserServiceTests : BaseBusinessModelServicesTest
    {
        [Test]
        public async Task CreateAdmin_TryoutRequiresAdministrators()
        {
            var userService = ObjectCreator.Create<IUserService>();
            Assert.That(!(await userService.GetAllUsers()).Any());

            Requires<Administrators>();

            var allUsers = await userService.GetAllUsers();
            Assert.That(allUsers.Any(user => user.IsAdmin));
        }

        [Test]
        public async Task CreateAdmin_TryoutRequiresRegularUsers()
        {
            var userService = ObjectCreator.Create<IUserService>();
            Assert.That(!(await userService.GetAllUsers()).Any());

            Requires<RegularUsers>();

            var allUsers = await userService.GetAllUsers();
            Assert.That(allUsers.Any());
            Assert.That(allUsers.All(user => !user.IsAdmin));
        }

        [Test]
        public async Task CreateAdmin_CreatesAdmin()
        {
            var userService = ObjectCreator.Create<IUserService>();

            var username = Guid.NewGuid() + "test";
            await userService.CreateAdmin(new NewUser
            {
                Username = username,
                FirstName = "TestName",
                LastName = "TestLastName"
            });

            Assert.That((await userService.GetAllUsers()).Any(user => user.Username == username));
        }
    }
}