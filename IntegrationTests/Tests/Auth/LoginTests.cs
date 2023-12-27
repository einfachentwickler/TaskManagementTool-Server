﻿using FluentAssertions;
using IntegrationTests.Constants;
using IntegrationTests.Utils;
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
    private TmtWebApplicationFactory application;
    private HttpClient client;

    [SetUp]
    public void Setup()
    {
        application = new TmtWebApplicationFactory();
        client = application.CreateClient();
    }

    [Test]
    public async Task LoginUser_ValidCredentials_Returns200()
    {
        //Arrange
        await TestsHelper.RegisterUserAsync(client, "user1@email.com", "password", "password");

        LoginDto loginDto = new()
        {
            Email = "user1@email.com",
            Password = "password"
        };

        //Act
        var response = await client.PostAsJsonAsync(UriConstants.LOGIN_URI, loginDto);

        //Assert
        response.EnsureSuccessStatusCode();

        var actualResult = await response.Content.ReadFromJsonAsync<UserManagerResponse>();

        actualResult!.Message.Should().NotBeNullOrEmpty();
        actualResult.IsSuccess.Should().BeTrue();
        actualResult.ExpiredDate!.Value.Should().BeAfter(DateTime.UtcNow);

        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + actualResult.Message);
        HttpResponseMessage getResponse = await TestsHelper.CreateTodoAsync(client);
        getResponse.EnsureSuccessStatusCode();
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
        var response = await client.PostAsJsonAsync(UriConstants.LOGIN_URI, loginDto);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        var actualResult = await response.Content.ReadFromJsonAsync<UserManagerResponse>();

        actualResult!.Message.Should().Be(UserManagerResponseMessages.USER_DOES_NOT_EXIST);
        actualResult.IsSuccess.Should().BeFalse();
    }

    [Test]
    public async Task LoginUser_UserIsBlocked_Returns401()
    {
        //Arrange
        await TestsHelper.RegisterUserAsync(client, "user1@email.com", "password", "password");
        await TestsHelper.LoginAsync(client, "admin@example.com", "password");

        HttpResponseMessage getResponse = await client.GetAsync(UriConstants.ADMIN_GET_USERS_URI + $"?pageSize={10}&pageNumber={1}");

        IEnumerable<UserDto>? users = await getResponse.Content.ReadFromJsonAsync<IEnumerable<UserDto>>();

        //Act
        HttpResponseMessage reverseStatusResponse = await client.PostAsync(UriConstants.REVERSE_STATUS_URI + users!.Single(x => x.Email == "user1@email.com").Id, null);

        //Assert
        reverseStatusResponse.EnsureSuccessStatusCode();

        LoginDto loginDto = new()
        {
            Email = "user1@email.com",
            Password = "password"
        };

        HttpResponseMessage loginResponse = await client.PostAsJsonAsync(UriConstants.LOGIN_URI, loginDto);

        loginResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        var actualResult = await loginResponse.Content.ReadFromJsonAsync<UserManagerResponse>();

        actualResult!.IsSuccess.Should().BeFalse();
        actualResult.Message.Should().Be(UserManagerResponseMessages.BLOCKED_EMAIL);
    }

    [Test]
    public async Task LoginUser_InvalidPassword_Returns401()
    {
        //Arrange
        await TestsHelper.RegisterUserAsync(client, "user1@email.com", "password", "password");

        LoginDto loginDto = new()
        {
            Email = "user1@email.com",
            Password = "password12"
        };

        //Act
        var response = await client.PostAsJsonAsync(UriConstants.LOGIN_URI, loginDto);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        var actualResult = await response.Content.ReadFromJsonAsync<UserManagerResponse>();

        actualResult!.Message.Should().Be(UserManagerResponseMessages.INVALID_CREDENTIALS);
        actualResult.IsSuccess.Should().BeFalse();
    }

    [TearDown]
    public async Task TearDownAsync()
    {
        client.Dispose();
        await application.DisposeAsync();
    }
}