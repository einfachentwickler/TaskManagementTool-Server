using IntegrationTests.SqlServer.EfCore.Configuration;
using Microsoft.AspNetCore.Identity;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        private const string PASSWORD = "password";

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
            string validId = await GetValidUserId();

            //act
            UserDto actualResult = await _instance.GetUserAsync(validId);

            //assert
            Assert.That(actualResult is not null);

            Assert.That(actualResult.Email is not null);
        }

        [Test]
        public async Task UpdateUserAsync_SuccessTest()
        {
            //arrange
            string validId = await GetValidUserId();

            User user = await TestStartup.UserManager.FindByIdAsync(validId);

            UserDto userToUpdate = TestStartup.Mapper.Map<User, UserDto>(user);

            string firstName = Guid.NewGuid().ToString();

            userToUpdate.FirstName = firstName;

            //act
            await _instance.UpdateUserAsync(userToUpdate);

            //assert
            User userAfterUpdate = await TestStartup.UserManager.FindByIdAsync(validId);

            Assert.That(userAfterUpdate is not null);

            Assert.That(userAfterUpdate.FirstName == firstName);
        }

        [Test]
        public async Task DeleteUserAsync_SuccessTest()
        {
            //arrange
            string email = $"{Guid.NewGuid()}@example.com";


            await RegisterTempUserAsync(email);

            User user = await TestStartup.UserManager.FindByEmailAsync(email);

            //act
            await _instance.DeleteUserAsync(user.Id);

            //assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => await _instance.GetUserAsync(user.Id));
        }

        private static async Task RegisterTempUserAsync(string email, bool isBlocked = false)
        {
            User registerUser = new()
            {
                Age = 14,
                Email = email,
                FirstName = "First name",
                LastName = "Last name",
                UserName = email,
                Role = UserRoles.USER_ROLE,
                IsBlocked = isBlocked
            };

            IdentityResult result = await TestStartup.UserManager.CreateAsync(registerUser, PASSWORD);

            if (!result.Succeeded)
            {
                throw new TaskManagementToolException(
                    $"User was not created: {string.Join("\n", result.Errors.Select(error => new { error.Code, error.Description }))}"
                );
            }
        }

        private static async Task<string> GetValidUserId()
        {
            User user = await TestStartup.UserManager
                .FindByEmailAsync(TestStartup.Configuration.GetSection("TestData:ValidEmail").Value);

            return user.Id;
        }
    }
}
