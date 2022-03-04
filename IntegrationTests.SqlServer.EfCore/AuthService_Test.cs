using IntegrationTests.SqlServer.EfCore.Configuration;
using Microsoft.AspNetCore.Identity;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementTool.BusinessLogic.Contracts;
using TaskManagementTool.BusinessLogic.Services;
using TaskManagementTool.BusinessLogic.ViewModels;
using TaskManagementTool.BusinessLogic.ViewModels.AuthModels;
using TaskManagementTool.Common.Constants;
using TaskManagementTool.Common.Exceptions;
using TaskManagementTool.DataAccess.Entities;

namespace IntegrationTests.SqlServer.EfCore
{
    [TestFixture]
    public class AuthService_Test
    {
        private IAuthService _instance;

        private const string PASSWORD = "password";

        [SetUp]
        public void Setup()
        {
            _instance = new AuthService(TestStartup.UserManager, TestStartup.Configuration);
        }

        [Test]
        public async Task LoginUserAsync_SuccessTest()
        {
            //arrange
            string email = $"{Guid.NewGuid()}@example.com";

            await RegisterAsyncTempUser(email);

            LoginDto model = new()
            {
                Email = email,
                Password = PASSWORD
            };

            //act
            UserManagerResponse actualResult = await _instance.LoginUserAsync(model);

            //assert
            Assert.That(actualResult.IsSuccess);

            await CleanupDatabase(email);
        }

        [Test]
        public async Task LoginUserAsync_BlockedTest()
        {
            //arrange
            string email = $"{Guid.NewGuid()}@example.com";

            await RegisterAsyncTempUser(email, true);

            LoginDto model = new()
            {
                Email = email,
                Password = PASSWORD
            };

            //act
            UserManagerResponse actualResult = await _instance.LoginUserAsync(model);

            //assert
            Assert.That(!actualResult.IsSuccess);
            Assert.That(actualResult.Message == "This email was blocked");

            await CleanupDatabase(email);
        }

        [Test]
        public async Task LoginUserAsync_WrongEmailTest()
        {
            //arrange
            string email = $"{Guid.NewGuid()}@example.com";

            LoginDto model = new()
            {
                Email = email,
                Password = PASSWORD
            };

            //act
            UserManagerResponse actualResult = await _instance.LoginUserAsync(model);

            //assert
            Assert.That(!actualResult.IsSuccess);
            Assert.That(actualResult.Message == "There is no user with this email");
        }

        [Test]
        public async Task LoginUserAsync_WrongPasswordTest()
        {
            //arrange
            LoginDto model = new()
            {
                Email = TestStartup.Configuration.GetSection("TestData:ValidEmail").Value,
                Password = PASSWORD + "wrong"
            };

            //act
            UserManagerResponse actualResult = await _instance.LoginUserAsync(model);

            //assert
            Assert.That(!actualResult.IsSuccess);
            Assert.That(actualResult.Message == "Incorrect login or password");
        }

        [Test]
        public async Task RegisterUserAsync_SuccessTest()
        {
            //arrange
            string email = $"{Guid.NewGuid()}@example.com";

            RegisterDto registerDto = new()
            {
                Age = 14,
                Password = "password",
                ConfirmPassword = "password",
                Email = email,
                FirstName = "First name",
                LastName = "Last name"
            };

            //act
            UserManagerResponse response = await _instance.RegisterUserAsync(registerDto);

            //assert
            Assert.That(response.IsSuccess);

            await CleanupDatabase(email);
        }

        [Test]
        public async Task RegisterUserAsync_PasswordDoesNotMatchTest()
        {
            //arrange
            string email = $"{Guid.NewGuid()}@example.com";

            RegisterDto registerDto = new()
            {
                Age = 14,
                Password = "password",
                ConfirmPassword = "password wrong",
                Email = email,
                FirstName = "First name",
                LastName = "Last name"
            };

            //act
            UserManagerResponse response = await _instance.RegisterUserAsync(registerDto);

            //assert
            Assert.That(!response.IsSuccess);
            Assert.That(response.Message == "Confirm password doesn't match the password");
        }

        private static async Task RegisterAsyncTempUser(string email, bool isBlocked = false)
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

        private static async Task CleanupDatabase(string email)
        {
            User user = await TestStartup.UserManager.FindByEmailAsync(email);

            await TestStartup.UserManager.DeleteAsync(user);
        }
    }
}
