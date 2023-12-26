using FluentAssertions;
using IntegrationTests.Constants;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using System.Net;
using System.Net.Http.Json;
using TaskManagementTool.BusinessLogic.Constants;
using TaskManagementTool.BusinessLogic.ViewModels;
using TaskManagementTool.BusinessLogic.ViewModels.AuthModels;

namespace IntegrationTests.Tests.Auth;

[TestFixture]
public class RegisterTests
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
    public async Task RegisterUserAsync_ValidData_Returns200()
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

        //Act
        var response = await _client.PostAsJsonAsync(UriConstants.REGISTER_URI, registerDto);

        //Assert
        response.EnsureSuccessStatusCode();

        var actualResult = await response.Content.ReadFromJsonAsync<UserManagerResponse>();

        actualResult!.Message.Should().Be(UserManagerResponseMessages.USER_CREATED);
        actualResult.IsSuccess.Should().BeTrue();
    }

    [Test]
    public async Task RegisterUserAsync_PasswordDoesNotMatch_Returns400()
    {
        //Arrange
        RegisterDto registerDto = new()
        {
            Age = 34,
            Password = "password",
            ConfirmPassword = "password2",
            Email = "user1@email.com",
            FirstName = "First name",
            LastName = "Last name"
        };

        //Act
        var response = await _client.PostAsJsonAsync(UriConstants.REGISTER_URI, registerDto);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var actualResult = await response.Content.ReadFromJsonAsync<UserManagerResponse>();

        actualResult!.Message.Should().Be(UserManagerResponseMessages.CONFIRM_PASSWORD_DOES_NOT_MATCH_PASSWORD);
        actualResult.IsSuccess.Should().BeFalse();
    }

    [Test]
    public async Task RegisterUserAsync_NullRequest_Returns400()
    {
        //Arrange
        RegisterDto? registerDto = null;

        //Act
        var response = await _client.PostAsJsonAsync(UriConstants.REGISTER_URI, registerDto);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

#warning TODO add error codes check
    }

    [Test, Ignore("TODO")]
    public async Task RegisterUserAsync_EmptyProperties_Returns400()
    {
        //Arrange
        RegisterDto? registerDto = new();

        //Act
        var response = await _client.PostAsJsonAsync(UriConstants.REGISTER_URI, registerDto);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

#warning TODO add error codes check
    }

    [Test, Ignore("TODO")]
    public async Task RegisterUserAsync_WeakPassword_Returns400()
    {
        //Arrange
        RegisterDto registerDto = new()
        {
            Age = 34,
            Password = "123",
            ConfirmPassword = "123",
            Email = "user1@email.com",
            FirstName = "First name",
            LastName = "Last name"
        };

        //Act
        var response = await _client.PostAsJsonAsync(UriConstants.REGISTER_URI, registerDto);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

#warning TODO add error codes check
    }

    [Test, Ignore("TODO")]
    public async Task RegisterUserAsync_InvalidEmail_Returns400()
    {
        //Arrange
        RegisterDto registerDto = new()
        {
            Age = 34,
            Password = "password",
            ConfirmPassword = "password",
            Email = "useremailcom",
            FirstName = "First name",
            LastName = "Last name"
        };

        //Act
        var response = await _client.PostAsJsonAsync(UriConstants.REGISTER_URI, registerDto);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

#warning TODO add error codes check
    }

    [TearDown]
    public async Task TearDownAsync()
    {
        _client.Dispose();
        await _application.DisposeAsync();
    }
}