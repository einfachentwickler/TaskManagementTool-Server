using IntegrationTests.SqlServer.EfCore.Configuration;
using IntegrationTests.SqlServer.EfCore.Constants;
using IntegrationTests.SqlServer.EfCore.Utils;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using TaskManagementTool.BusinessLogic.Constants;
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
            const bool isBlocked = false;

            await TestUserDatabaseUtils.RegisterTempUserAsync(email, isBlocked);

            LoginDto model = new()
            {
                Email = email,
                Password = MockDataConstants.TEMP_USER_PASSWORD
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
            const bool isBlocked = true;

            await TestUserDatabaseUtils.RegisterTempUserAsync(email, isBlocked);

            LoginDto model = new()
            {
                Email = email,
                Password = MockDataConstants.TEMP_USER_PASSWORD
            };

            //act
            UserManagerResponse actualResult = await _instance.LoginUserAsync(model);

            //assert
            Assert.That(!actualResult.IsSuccess);
            Assert.That(actualResult.Message == UserManagerResponseMessages.BLOCKED_EMAIL);

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
                Password = MockDataConstants.TEMP_USER_PASSWORD
            };

            //act
            UserManagerResponse actualResult = await _instance.LoginUserAsync(model);

            //assert
            Assert.That(!actualResult.IsSuccess);
            Assert.That(actualResult.Message == UserManagerResponseMessages.USER_DOES_NOT_EXIST);
        }

        [Test]
        public async Task LoginUserAsync_WrongPasswordTest()
        {
            //arrange
            string email = $"{Guid.NewGuid()}@example.com";
            const bool isBlocked = false;

            await TestUserDatabaseUtils.RegisterTempUserAsync(email, isBlocked);

            LoginDto model = new()
            {
                Email = email,
                Password = MockDataConstants.TEMP_USER_PASSWORD + "wrong"
            };

            //act
            UserManagerResponse actualResult = await _instance.LoginUserAsync(model);

            //assert
            Assert.That(!actualResult.IsSuccess);
            Assert.That(actualResult.Message == UserManagerResponseMessages.INVALID_CREDENTIALS);

            await TestUserDatabaseUtils.CleanupDatabase(email);
        }

        [Test]
        public async Task RegisterUserAsync_SuccessTest()
        {
            //arrange
            string email = $"{Guid.NewGuid()}@example.com";
            const bool confirmPasswordSuccesses = true;

            RegisterDto registerDto = TestUserDatabaseUtils.GetRegisterDto(email, confirmPasswordSuccesses);

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
            const bool confirmPasswordSuccesses = false;

            RegisterDto registerDto = TestUserDatabaseUtils.GetRegisterDto(email, confirmPasswordSuccesses);

            //act
            UserManagerResponse response = await _instance.RegisterUserAsync(registerDto);

            //assert
            Assert.That(!response.IsSuccess);
            Assert.That(response.Message == UserManagerResponseMessages.CONFIRM_PASSWORD_DOES_NOT_MATCH_PASSWORD);
        }
    }
}
