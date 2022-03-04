using IntegrationTests.SqlServer.EfCore.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
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

        private UserManager<User> UserManager;

        private const string PASSWORD = "password";

        [SetUp]
        public void Setup()
        {
            IUserStore<User> userStore = new UserStore<User>(TestStartup.Dao);

            IPasswordHasher<User> hasher = new PasswordHasher<User>();

            UserValidator<User> validator = new();
            List<UserValidator<User>> validators = new() { validator };

            ILogger<UserManager<User>> logger = new Mock<ILogger<UserManager<User>>>().Object;

            UserManager = new UserManager<User>(
                userStore,
                null,
                hasher, validators,
                null,
                null,
                null,
                null,
                logger
                );

            // Set-up token providers.
            IUserTwoFactorTokenProvider<User> tokenProvider = new EmailTokenProvider<User>();
            UserManager.RegisterTokenProvider("Default", tokenProvider);
            IUserTwoFactorTokenProvider<User> phoneTokenProvider = new PhoneNumberTokenProvider<User>();
            UserManager.RegisterTokenProvider("PhoneTokenProvider", phoneTokenProvider);

            _instance = new AuthService(UserManager, TestStartup.Configuration);
        }

        [Test]
        public async Task LoginUserAsync_SuccessTest()
        {
            string email = $"{Guid.NewGuid()}@example.com";

            await RegisterAsyncTempUser(email);

            LoginDto model = new()
            {
                Email = email,
                Password = PASSWORD
            };

            UserManagerResponse actualResult = await _instance.LoginUserAsync(model);

            Assert.That(actualResult.IsSuccess);

            await CleanupDatabase(email);
        }

        [Test]
        public async Task LoginUserAsync_BlockedTest()
        {
            string email = $"{Guid.NewGuid()}@example.com";

            await RegisterAsyncTempUser(email, true);

            LoginDto model = new()
            {
                Email = email,
                Password = PASSWORD
            };

            UserManagerResponse actualResult = await _instance.LoginUserAsync(model);

            Assert.That(!actualResult.IsSuccess);
            Assert.That(actualResult.Message == "This email was blocked");

            await CleanupDatabase(email);
        }

        [Test]
        public async Task LoginUserAsync_WrongEmailTest()
        {
            string email = $"{Guid.NewGuid()}@example.com";

            LoginDto model = new()
            {
                Email = email,
                Password = PASSWORD
            };

            UserManagerResponse actualResult = await _instance.LoginUserAsync(model);

            Assert.That(!actualResult.IsSuccess);
            Assert.That(actualResult.Message == "There is no user with this email");
        }

        [Test]
        public async Task LoginUserAsync_WrongPasswordTest()
        {
            LoginDto model = new()
            {
                Email = TestStartup.Configuration.GetSection("TestData:ValidEmail").Value,
                Password = PASSWORD + "wrong"
            };

            UserManagerResponse actualResult = await _instance.LoginUserAsync(model);

            Assert.That(!actualResult.IsSuccess);
            Assert.That(actualResult.Message == "Incorrect login or password");
        }

        [Test]
        public async Task RegisterUserAsync_SuccessTest()
        {
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

            UserManagerResponse response = await _instance.RegisterUserAsync(registerDto);

            Assert.That(response.IsSuccess);

            await CleanupDatabase(email);
        }

        [Test]
        public async Task RegisterUserAsync_PasswordDoesNotMatchTest()
        {
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

            UserManagerResponse response = await _instance.RegisterUserAsync(registerDto);

            Assert.That(!response.IsSuccess);
            Assert.That(response.Message == "Confirm password doesn't match the password");
        }

        private async Task RegisterAsyncTempUser(string email, bool isBlocked = false)
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

            IdentityResult result = await UserManager.CreateAsync(registerUser, PASSWORD);

            if (!result.Succeeded)
            {
                throw new TaskManagementToolException(
                    $"User was not created: {string.Join("\n", result.Errors.Select(error => new { error.Code, error.Description }))}"
                    );
            }
        }

        private async Task CleanupDatabase(string email)
        {
            User user = await UserManager.FindByEmailAsync(email);

            await UserManager.DeleteAsync(user);
        }
    }
}
