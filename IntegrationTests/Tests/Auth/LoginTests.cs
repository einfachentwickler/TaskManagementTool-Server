using FluentAssertions;
using IntegrationTests.Constants;
using NUnit.Framework;
using System.Net;
using System.Net.Http.Json;
using TaskManagementTool.BusinessLogic.Constants;
using TaskManagementTool.BusinessLogic.ViewModels;
using TaskManagementTool.BusinessLogic.ViewModels.AuthModels;

namespace IntegrationTests.Tests.Auth;

[TestFixture]
public class LoginTests
{
    private TmtWebApplicationFactory _application;
    private HttpClient _client;

    [SetUp]
    public void Setup()
    {
        _application = new TmtWebApplicationFactory();
        _client = _application.CreateClient();
    }

    [Test]
    public async Task LoginUser_ValidCredentials_Returns200()
    {
        //Arrange
        RegisterDto registerDto = new()
        {
            Age = 34,
            Password = "password",
            ConfirmPassword = "password",
            Email = "user1@email.com",
            FirstName = "First name",
            LastName = "Last name"
        };

        await _client.PostAsJsonAsync(UriConstants.REGISTER_URI, registerDto);

        LoginDto loginDto = new()
        {
            Email = "user1@email.com",
            Password = "password"
        };

        //Act
        var response = await _client.PostAsJsonAsync(UriConstants.LOGIN_URI, loginDto);

        //Assert
        response.EnsureSuccessStatusCode();

        var actualResult = await response.Content.ReadFromJsonAsync<UserManagerResponse>();

#warning TODO to check if this token is valid add request for any todo and check that response code is not 401
        actualResult!.Message.Should().NotBeNullOrEmpty();
        actualResult.IsSuccess.Should().BeTrue();
        actualResult.ExpiredDate!.Value.Should().BeAfter(DateTime.UtcNow);
    }

    [Test]
    public async Task LoginUser_UserDoesNotExist_Returns401()
    {
        //Arrange
        LoginDto loginDto = new()
        {
            Email = "user1@email.com",
            Password = "password"
        };

        //Act
        var response = await _client.PostAsJsonAsync(UriConstants.LOGIN_URI, loginDto);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        var actualResult = await response.Content.ReadFromJsonAsync<UserManagerResponse>();

        actualResult!.Message.Should().Be(UserManagerResponseMessages.USER_DOES_NOT_EXIST);
        actualResult.IsSuccess.Should().BeFalse();
    }

#warning TODO Test case blocked user

    [Test]
    public async Task LoginUser_InvalidPassword_Returns401()
    {
        //Arrange
        RegisterDto registerDto = new()
        {
            Age = 34,
            Password = "password",
            ConfirmPassword = "password",
            Email = "user1@email.com",
            FirstName = "First name",
            LastName = "Last name"
        };

        await _client.PostAsJsonAsync(UriConstants.REGISTER_URI, registerDto);

        LoginDto loginDto = new()
        {
            Email = "user1@email.com",
            Password = "password12"
        };

        //Act
        var response = await _client.PostAsJsonAsync(UriConstants.LOGIN_URI, loginDto);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        var actualResult = await response.Content.ReadFromJsonAsync<UserManagerResponse>();

        actualResult!.Message.Should().Be(UserManagerResponseMessages.INVALID_CREDENTIALS);
        actualResult.IsSuccess.Should().BeFalse();
    }

    [TearDown]
    public async Task TearDownAsync()
    {
        _client.Dispose();
        await _application.DisposeAsync();
    }
}