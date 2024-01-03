using FluentAssertions;
using IntegrationTests.Constants;
using IntegrationTests.Utils;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using System.Net;
using System.Net.Http.Json;
using TaskManagementTool.BusinessLogic.Constants;
using TaskManagementTool.BusinessLogic.Dto.Errors;
using TaskManagementTool.BusinessLogic.ViewModels;
using TaskManagementTool.BusinessLogic.ViewModels.AuthModels;
using TaskManagementTool.Common.Enums;

namespace IntegrationTests.Tests.Auth;

[TestFixture]
public class RegisterTests
{
    private TmtWebApplicationFactory _application;
    private HttpClient _client;

    const string EMAIL = "user1@email.com";
    const string PASSWORD = "password";

    [SetUp]
    public void Setup()
    {
        _application = new TmtWebApplicationFactory();
        _client = _application.CreateClient();
    }

    [Test]
    public async Task RegisterUserAsync_ValidData_Returns200()
    {
        //Act
        var response = await TestsHelper.RegisterUserAsync(_client, EMAIL, PASSWORD, PASSWORD);

        //Assert
        response.EnsureSuccessStatusCode();

        var actualResult = await response.Content.ReadFromJsonAsync<UserManagerResponse>();

        actualResult!.Message.Should().Be(UserManagerResponseMessages.USER_CREATED);
        actualResult.IsSuccess.Should().BeTrue();
    }

    [Test]
    public async Task RegisterUserAsync_PasswordDoesNotMatch_Returns400()
    {
        //Act
        var response = await TestsHelper.RegisterUserAsync(_client, EMAIL, PASSWORD, "nonsence");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var actualResult = await response.Content.ReadFromJsonAsync<ErrorDto>();

        actualResult!.ErrorCode.Should().Be(ApiErrorCode.InvalidInput);
        actualResult.ErrorMessage.Should().Be("ConfirmPasswordDoesNotMatch");
    }

    [Test]
    public async Task RegisterUserAsync_UserAlreadyExists_Returns400()
    {
        //Arrange
        await TestsHelper.RegisterUserAsync(_client, EMAIL, PASSWORD, PASSWORD);

        //Act
        HttpResponseMessage response = await TestsHelper.RegisterUserAsync(_client, EMAIL, PASSWORD, PASSWORD);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var actualResult = await response.Content.ReadFromJsonAsync<UserManagerResponse>();

        actualResult!.Message.Should().Be(UserManagerResponseMessages.USER_WAS_NOT_CREATED);
        actualResult.IsSuccess.Should().BeFalse();

        actualResult.Errors.Should().Contain($"Username '{EMAIL}' is already taken.");
    }

    [Test]
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

    [Test]
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