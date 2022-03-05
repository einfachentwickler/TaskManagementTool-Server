using IntegrationTests.SqlServer.EfCore.Configuration;
using IntegrationTests.SqlServer.EfCore.Utils;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using TaskManagementTool.BusinessLogic.Contracts;
using TaskManagementTool.BusinessLogic.Services;
using TaskManagementTool.BusinessLogic.ViewModels;
using TaskManagementTool.BusinessLogic.ViewModels.AuthModels;

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

            await TestUserDatabaseUtils.RegisterTempUserAsync(email);

            LoginDto model = new()
            {
                Email = email,
                Password = PASSWORD
            };

            //act
            UserManagerResponse actualResult = await _instance.LoginUserAsync(model);

            //assert
            Assert.That(actualResult.IsSuccess);

            await TestUserDatabaseUtils.CleanupDatabase(email);
        }

        [Test]
        public async Task LoginUserAsync_BlockedTest()
        {
            //arrange
            string email = $"{Guid.NewGuid()}@example.com";

            await TestUserDatabaseUtils.RegisterTempUserAsync(email, true);

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

            await TestUserDatabaseUtils.CleanupDatabase(email);
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
            string email = $"{Guid.NewGuid()}@example.com";
            await TestUserDatabaseUtils.RegisterTempUserAsync(email);

            LoginDto model = new()
            {
                Email = email,
                Password = PASSWORD + "wrong"
            };

            //act
            UserManagerResponse actualResult = await _instance.LoginUserAsync(model);

            //assert
            Assert.That(!actualResult.IsSuccess);
            Assert.That(actualResult.Message == "Incorrect login or password");

            await TestUserDatabaseUtils.CleanupDatabase(email);
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

            await TestUserDatabaseUtils.CleanupDatabase(email);
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
    }
}
