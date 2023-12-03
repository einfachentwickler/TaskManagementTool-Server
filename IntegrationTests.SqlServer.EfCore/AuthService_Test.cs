using IntegrationTests.SqlServer.EfCore.Configuration;
using IntegrationTests.SqlServer.EfCore.Constants;
using IntegrationTests.SqlServer.EfCore.Utils;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using TaskManagementTool.BusinessLogic.Constants;
using TaskManagementTool.BusinessLogic.Services;
using TaskManagementTool.BusinessLogic.ViewModels;
using TaskManagementTool.BusinessLogic.ViewModels.AuthModels;

namespace IntegrationTests.SqlServer.EfCore;

[TestFixture]
public class AuthService_Test
{
    private AuthHandler _instance;

    [SetUp]
    public void Setup()
    {
        _instance = new AuthHandler(TestStartup.UserManager, TestStartup.Configuration);
    }

    [Test]
    public async Task LoginUserAsync_Success_ReturnsSuccess()
    {
        //Arrange
        string email = $"{Guid.NewGuid()}@example.com";

        await TestUserDatabaseUtils.RegisterTempUserAsync(email, false);

        LoginDto model = new()
        {
            Email = email,
            Password = MockDataConstants.TEMP_USER_PASSWORD
        };

        //Act
        UserManagerResponse actualResult = await _instance.LoginUserAsync(model);

        //Assert
        Assert.That(actualResult.IsSuccess, Is.True);
    }

    [Test]
    public async Task LoginUserAsync_Blocked_ReturnsFailure()
    {
        //Arrange
        string email = $"{Guid.NewGuid()}@example.com";

        await TestUserDatabaseUtils.RegisterTempUserAsync(email, true);

        LoginDto model = new()
        {
            Email = email,
            Password = MockDataConstants.TEMP_USER_PASSWORD
        };

        //Act
        UserManagerResponse actualResult = await _instance.LoginUserAsync(model);

        //Assert
        Assert.That(actualResult.IsSuccess, Is.False);
        Assert.That(actualResult.Message, Is.EqualTo(UserManagerResponseMessages.BLOCKED_EMAIL));
    }

    [Test]
    public async Task LoginUserAsync_WrongEmail_ReturnsFailure()
    {
        //Arrange
        string email = $"{Guid.NewGuid()}@example.com";

        LoginDto model = new()
        {
            Email = email,
            Password = MockDataConstants.TEMP_USER_PASSWORD
        };

        //Act
        UserManagerResponse actualResult = await _instance.LoginUserAsync(model);

        //Assert
        Assert.That(actualResult.IsSuccess, Is.False);
        Assert.That(actualResult.Message, Is.EqualTo(UserManagerResponseMessages.USER_DOES_NOT_EXIST));
    }

    [Test]
    public async Task LoginUserAsync_WrongPassword_ReturnsFailure()
    {
        //Arrange
        string email = $"{Guid.NewGuid()}@example.com";

        await TestUserDatabaseUtils.RegisterTempUserAsync(email, false);

        LoginDto model = new()
        {
            Email = email,
            Password = MockDataConstants.TEMP_USER_PASSWORD + "wrong"
        };

        //Act
        UserManagerResponse actualResult = await _instance.LoginUserAsync(model);

        //Assert
        Assert.That(actualResult.IsSuccess, Is.False);
        Assert.That(actualResult.Message, Is.EqualTo(UserManagerResponseMessages.INVALID_CREDENTIALS));
    }

    [Test]
    public async Task RegisterUserAsync_Success_ReturnsSuccess()
    {
        //Arrange
        string email = $"{Guid.NewGuid()}@example.com";

        RegisterDto registerDto = TestUserDatabaseUtils.GetRegisterDto(email, true);

        //Act
        UserManagerResponse response = await _instance.RegisterUserAsync(registerDto);

        //Assert
        Assert.That(response.IsSuccess, Is.True);
    }

    [Test]
    public async Task RegisterUserAsync_PasswordDoesNotMatch_ReturnsFailure()
    {
        //Arrange
        string email = $"{Guid.NewGuid()}@example.com";

        RegisterDto registerDto = TestUserDatabaseUtils.GetRegisterDto(email, false);

        //Act
        UserManagerResponse response = await _instance.RegisterUserAsync(registerDto);

        //Assert
        Assert.That(response.IsSuccess, Is.False);
        Assert.That(response.Message, Is.EqualTo(UserManagerResponseMessages.CONFIRM_PASSWORD_DOES_NOT_MATCH_PASSWORD));
    }
}