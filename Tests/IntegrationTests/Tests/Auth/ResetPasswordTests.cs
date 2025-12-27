using Application.Commands.Auth.Login.Models;
using Application.Commands.Auth.Register.Models;
using Application.Commands.Auth.ResetPassword.Models;
using Azure;
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
public class ResetPasswordTests
{
    private TmtWebApplicationFactory application;
    private HttpClient client;

    const string EMAIL = "user1@email.com";
    const string PASSWORD = "password";

    [SetUp]
    public void Setup()
    {
        application = new TmtWebApplicationFactory();
        client = application.CreateClient();
    }

    [Test]
    public async Task ResetPasswordAsync_ValidData_Returns200()
    {
        //Arrange
        await TestsHelper.RegisterUserAsync(client, EMAIL, PASSWORD, PASSWORD);

        const string newPassword = "Qwerty123$";

        var request = new ResetPasswordCommand
        {
            Email = EMAIL,
            CurrentPassword = PASSWORD,
            NewPassword = newPassword,
            ConfirmNewPassword = newPassword
        };

        //Act
        var actualResponse = await client.PostAsJsonAsync(UriConstants.AUTH_RESET_PASSWORD_URI, request);

        //Assert
        actualResponse.EnsureSuccessStatusCode();

        var loginDto = new UserLoginCommand()
        {
            Email = EMAIL,
            Password = PASSWORD
        };

        var loginResponseWithOldCredentials = await client.PostAsJsonAsync(UriConstants.AUTH_LOGIN_URI, loginDto);
        loginResponseWithOldCredentials.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        await TestsHelper.LoginAsync(client, EMAIL, newPassword);

        var getResponseWithNewCredentials = await client.GetAsync(string.Format(UriConstants.HOME_GET_USER_TODOS, 1, 10));
        getResponseWithNewCredentials.EnsureSuccessStatusCode();
    }

    [Test]
    public async Task ResetPasswordAsync_InvalidCurrentPassword_Returns400()
    {
        //Arrange
        await TestsHelper.RegisterUserAsync(client, EMAIL, PASSWORD, PASSWORD);

        const string newPassword = "Qwerty123$";

        var request = new ResetPasswordCommand
        {
            Email = EMAIL,
            CurrentPassword = PASSWORD + "qwerty",
            NewPassword = newPassword,
            ConfirmNewPassword = newPassword
        };

        //Act
        HttpResponseMessage actualResponse = await client.PostAsJsonAsync(UriConstants.AUTH_RESET_PASSWORD_URI, request);

        //Assert
        actualResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var actualResult = await actualResponse.Content.ReadFromJsonAsync<ProblemDetails>();

        actualResult.Should().BeEquivalentTo(new ProblemDetails
        {
            Title = nameof(ResetPasswordErrorCode.InvalidCurrentPassword),
            Status = StatusCodes.Status400BadRequest,
            Detail = ResetPasswordErrorMessages.InvalidCurrentPassword
        }, options => options.Excluding(dto => dto.Extensions));
    }

    [Test]
    public async Task ResetPasswordAsync_NewPasswordDoesNotMatchConfirmPassword_Returns400()
    {
        //Arrange
        await TestsHelper.RegisterUserAsync(client, EMAIL, PASSWORD, PASSWORD);

        const string newPassword = "Qwerty123$";

        var request = new ResetPasswordCommand
        {
            Email = EMAIL,
            CurrentPassword = PASSWORD,
            NewPassword = newPassword,
            ConfirmNewPassword = newPassword + "1"
        };

        //Act
        var actualResponse = await client.PostAsJsonAsync(UriConstants.AUTH_RESET_PASSWORD_URI, request);

        //Assert
        actualResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var actualResult = await actualResponse.Content.ReadFromJsonAsync<ProblemDetails>();

        actualResult.Should().BeEquivalentTo(new ProblemDetails
        {
            Title = nameof(ResetPasswordErrorCode.PasswordsDoNotMatch),
            Status = StatusCodes.Status400BadRequest,
            Detail = ResetPasswordErrorMessages.PasswordsDoNotMatch
        }, options => options.Excluding(dto => dto.Extensions));
    }

    [TearDown]
    public async Task TearDownAsync()
    {
        client.Dispose();
        await application.DisposeAsync();
    }
}