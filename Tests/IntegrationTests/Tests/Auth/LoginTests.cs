using Application.Commands.Auth.Login.Models;
using Application.Commands.Auth.Login.Validation;
using Application.Queries.Admin.GetUsers.Models;
using FluentAssertions;
using IntegrationTests.Constants;
using IntegrationTests.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System.Net;
using System.Net.Http.Json;

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
    public async Task LoginUser_ValidCredentials_Returns400()
    {
        //Arrange
        await TestsHelper.RegisterUserAsync(_client, "user1@email.com", "password", "password");

        var loginDto = new UserLoginCommand
        {
            Email = "user1@email.com",
            Password = "password"
        };

        //Act
        var response = await _client.PostAsJsonAsync(UriConstants.AUTH_LOGIN_URI, loginDto);

        //Assert
        response.EnsureSuccessStatusCode();

        var actualResult = await response.Content.ReadFromJsonAsync<UserLoginResponse>();

        actualResult!.Token.Should().NotBeNullOrEmpty();
        actualResult.Expires.Should().BeAfter(DateTime.UtcNow);

        _client.DefaultRequestHeaders.Clear();

        _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + actualResult.Token);
        var getResponse = await TestsHelper.CreateTodoAsync(_client);

        getResponse.EnsureSuccessStatusCode();
    }

    [Test]
    public async Task LoginUser_UserDoesNotExist_Returns400()
    {
        //Arrange
        var loginDto = new UserLoginCommand
        {
            Email = "user1@email.com",
            Password = "password"
        };

        //Act
        var response = await _client.PostAsJsonAsync(UriConstants.AUTH_LOGIN_URI, loginDto);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var actualResult = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        actualResult.Should().BeEquivalentTo(new ProblemDetails
        {
            Title = nameof(UserLoginErrorCode.InvalidCredentials),
            Status = StatusCodes.Status400BadRequest,
            Detail = UserLoginErrorMessages.InvalidCredentials
        }, options => options.Excluding(dto => dto.Extensions));
    }

    [Test]
    public async Task LoginUser_UserIsBlocked_Returns400()
    {
        //Arrange
        await TestsHelper.RegisterUserAsync(_client, "user1@email.com", "password", "password");
        await TestsHelper.LoginAsync(_client, "admin@example.com", "password");

        var getResponse = await _client.GetAsync(string.Format(UriConstants.ADMIN_GET_USERS_URI, 10, 1));
        var users = await getResponse.Content.ReadFromJsonAsync<GetUsersResponse>();

        var reverseStatusResponse = await _client.PostAsync(UriConstants.ADMIN_REVERSE_STATUS_URI + users!.Users.Single(x => x.Email == "user1@email.com").Id, null);
        reverseStatusResponse.EnsureSuccessStatusCode();

        var loginDto = new UserLoginCommand
        {
            Email = "user1@email.com",
            Password = "password"
        };

        //Act
        var loginResponse = await _client.PostAsJsonAsync(UriConstants.AUTH_LOGIN_URI, loginDto);

        //Assert
        loginResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var actualResult = await loginResponse.Content.ReadFromJsonAsync<ProblemDetails>();

        actualResult.Should().BeEquivalentTo(new ProblemDetails
        {
            Title = nameof(UserLoginErrorCode.BlockedEmail),
            Status = StatusCodes.Status400BadRequest,
            Detail = UserLoginErrorMessages.BlockedEmail
        }, options => options.Excluding(dto => dto.Extensions));
    }

    [Test]
    public async Task LoginUser_InvalidPassword_Returns401()
    {
        //Arrange
        await TestsHelper.RegisterUserAsync(_client, "user1@email.com", "password", "password");

        var loginDto = new UserLoginCommand
        {
            Email = "user1@email.com",
            Password = "password12"
        };

        //Act
        var response = await _client.PostAsJsonAsync(UriConstants.AUTH_LOGIN_URI, loginDto);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var actualResult = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        actualResult.Should().BeEquivalentTo(new ProblemDetails
        {
            Title = nameof(UserLoginErrorCode.InvalidCredentials),
            Status = StatusCodes.Status400BadRequest,
            Detail = UserLoginErrorMessages.InvalidCredentials
        }, options => options.Excluding(dto => dto.Extensions));
    }

    [Test]
    public async Task LoginUser_RequestContainsNoBody_Returns400()
    {
        //Arrange
        UserLoginCommand? loginDto = null;

        //Act
        var response = await _client.PostAsJsonAsync(UriConstants.AUTH_LOGIN_URI, loginDto);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [TearDown]
    public async Task TearDownAsync()
    {
        _client.Dispose();
        await _application.DisposeAsync();
    }
}