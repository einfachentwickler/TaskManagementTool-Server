using IntegrationTests.SqlServer.EfCore.Configuration;
using IntegrationTests.SqlServer.EfCore.Constants;
using Microsoft.AspNetCore.Identity;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IntegrationTests.SqlServer.EfCore.Utils;
using TaskManagementTool.BusinessLogic.Contracts;
using TaskManagementTool.BusinessLogic.Services;
using TaskManagementTool.BusinessLogic.ViewModels;
using TaskManagementTool.Common.Constants;
using TaskManagementTool.Common.Exceptions;
using TaskManagementTool.DataAccess.Entities;
using TaskManagementTool.DataAccess.Repositories;

namespace IntegrationTests.SqlServer.EfCore
{
    [TestFixture]
    public class AdminService_Tests
    {
        private IAdminService _instance;

        [SetUp]
        public void Setup()
        {
            _instance = new AdminService(TestStartup.Mapper, TestStartup.UserManager, new TodoRepository(TestStartup.Dao));
        }

        [Test]
        public async Task GetUsers_SuccessTest()
        {
            IEnumerable<UserDto> actualResult = await _instance.GetUsersAsync();

            Assert.That(actualResult.Any());
        }

        [Test]
        public async Task GetUser_SuccessTest()
        {
            //arrange
            string email = $"{Guid.NewGuid()}@example.com";

            await TestUserDatabaseUtils.RegisterTempUserAsync(email);

            User user = await TestStartup.UserManager.FindByEmailAsync(email);

            //act
            UserDto actualResult = await _instance.GetUserAsync(user.Id);

            //assert
            Assert.That(actualResult is not null);

            Assert.That(actualResult.Email is not null);

            await TestUserDatabaseUtils.CleanupDatabase(email);
        }

        [Test]
        public async Task UpdateUserAsync_SuccessTest()
        {
            //arrange
            string email = $"{Guid.NewGuid()}@example.com";

            await TestUserDatabaseUtils.RegisterTempUserAsync(email);

            User user = await TestStartup.UserManager.FindByEmailAsync(email);

            UserDto userToUpdate = TestStartup.Mapper.Map<User, UserDto>(user);

            string firstName = Guid.NewGuid().ToString();

            userToUpdate.FirstName = firstName;

            //act
            await _instance.UpdateUserAsync(userToUpdate);

            //assert
            User userAfterUpdate = await TestStartup.UserManager.FindByEmailAsync(email);

            Assert.That(userAfterUpdate is not null);

            Assert.That(userAfterUpdate.FirstName == firstName);

            await TestUserDatabaseUtils.CleanupDatabase(email);
        }

        [Test]
        public async Task DeleteUserAsync_SuccessTest()
        {
            //arrange
            string email = $"{Guid.NewGuid()}@example.com";

            await TestUserDatabaseUtils.RegisterTempUserAsync(email);

            User user = await TestStartup.UserManager.FindByEmailAsync(email);

            //act
            await _instance.DeleteUserAsync(user.Id);

            //assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => await _instance.GetUserAsync(user.Id));
        }
    }
}
