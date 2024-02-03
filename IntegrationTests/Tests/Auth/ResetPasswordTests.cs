﻿using FluentAssertions;
using IntegrationTests.Constants;
using IntegrationTests.Utils;
using NUnit.Framework;
using System.Net;
using System.Net.Http.Json;
using TaskManagementTool.BusinessLogic.Dto.AuthModels;
using TaskManagementTool.BusinessLogic.ViewModels;
using TaskManagementTool.BusinessLogic.ViewModels.AuthModels;

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

        ResetPasswordDto request = new()
        {
            Email = EMAIL,
            CurrentPassword = PASSWORD,
            NewPassword = newPassword,
            ConfirmNewPassword = newPassword
        };

        //Act
        HttpResponseMessage actualResponse = await client.PostAsJsonAsync(UriConstants.RESET_PASSWORD_URI, request);

        //Assert
        actualResponse.EnsureSuccessStatusCode();

        var actualResult = await actualResponse.Content.ReadFromJsonAsync<UserManagerResponse>();

        actualResult.Should().BeEquivalentTo(new UserManagerResponse { IsSuccess = true });

        LoginDto loginDto = new()
        {
            Email = EMAIL,
            Password = PASSWORD
        };

        HttpResponseMessage loginResponseWithOldCredentials = await client.PostAsJsonAsync(UriConstants.LOGIN_URI, loginDto);
        loginResponseWithOldCredentials.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        await TestsHelper.LoginAsync(client, EMAIL, newPassword);
        HttpResponseMessage getResponseWithNewCredentials = await client.GetAsync(UriConstants.GET_TODO_URI);
        getResponseWithNewCredentials.EnsureSuccessStatusCode();
    }

    [Test]
    public async Task ResetPasswordAsync_InvalidCurrentPassword_Returns401()
    {
        //Arrange
        await TestsHelper.RegisterUserAsync(client, EMAIL, PASSWORD, PASSWORD);

        const string newPassword = "Qwerty123$";

        ResetPasswordDto request = new()
        {
            Email = EMAIL,
            CurrentPassword = PASSWORD + "qwerty",
            NewPassword = newPassword,
            ConfirmNewPassword = newPassword
        };

        //Act
        HttpResponseMessage actualResponse = await client.PostAsJsonAsync(UriConstants.RESET_PASSWORD_URI, request);

        //Assert
        actualResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}