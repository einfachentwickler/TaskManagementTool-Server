﻿using FluentAssertions;
using IntegrationTests.Constants;
using IntegrationTests.Utils;
using NUnit.Framework;
using System.Net;
using System.Net.Http.Json;
using TaskManagementTool.BusinessLogic.Dto.Errors;
using TaskManagementTool.BusinessLogic.ViewModels;
using TaskManagementTool.Common.Enums;

namespace IntegrationTests.Tests.Admin;

[TestFixture]
public class DeleteUserTests
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
    public async Task DeleteUser_ValidPath_UserAndHisTodosAreDeleted()
    {
        //Arrange
        await TestsHelper.RegisterUserAsync(client, "user1@email.com", "password", "password");
        await TestsHelper.RegisterUserAsync(client, "user2@email.com", "password", "password");

        await TestsHelper.LoginAsync(client, "user1@email.com", "password");
        await TestsHelper.CreateTodoAsync(client, "Todo 1");
        await TestsHelper.CreateTodoAsync(client, "Todo 2");
        await TestsHelper.CreateTodoAsync(client, "Todo 3");

        await TestsHelper.LoginAsync(client, "user2@email.com", "password");
        await TestsHelper.CreateTodoAsync(client, "Todo 4");
        await TestsHelper.CreateTodoAsync(client, "Todo 5");
        await TestsHelper.CreateTodoAsync(client, "Todo 6");

        await TestsHelper.LoginAsync(client, "admin@example.com", "password");

        //Act
        HttpResponseMessage response = await client.DeleteAsync(UriConstants.ADMIN_DELETE_USER_URI + "user2@email.com");

        //Assert
        response.EnsureSuccessStatusCode();

        HttpResponseMessage getUsersResponse = await client.GetAsync(UriConstants.ADMIN_GET_USERS_URI + "?pageSize=10&pageNumber=1");
        IEnumerable<UserDto>? users = await getUsersResponse.Content.ReadFromJsonAsync<IEnumerable<UserDto>>();
        users.Should().HaveCount(2)
            .And.Contain(x => x.Email == "admin@example.com")
            .And.Contain(x => x.Email == "user1@email.com");

        HttpResponseMessage getTodosResponse = await client.GetAsync(UriConstants.ADMIN_GET_TODOS_URI + "?pageSize=10&pageNumber=1");
        IEnumerable<TodoDto>? todos = await getTodosResponse.Content.ReadFromJsonAsync<IEnumerable<TodoDto>>();
        todos.Should().HaveCount(5)
            .And.Contain(dto => dto.Name == "Todo 1")
            .And.Contain(dto => dto.Name == "Todo 2")
            .And.Contain(dto => dto.Name == "Todo 3");
    }

    [Test]
    public async Task DeleteUser_UserDoesNotExist_ReturnsNotFound()
    {
        //Arrange
        await TestsHelper.LoginAsync(client, "admin@example.com", "password");

        //Act
        HttpResponseMessage response = await client.DeleteAsync(UriConstants.ADMIN_DELETE_USER_URI + "user2@email.com");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var actualResult = await response.Content.ReadFromJsonAsync<ErrorDto>();

        actualResult.Should().BeEquivalentTo(new ErrorDto(ApiErrorCode.UserNotFound, "User with email user2@email.com was not found"));
    }

    [Test]
    public async Task DeleteUser_NoPermission_ReturnsForbidden()
    {
        //Arrange
        await TestsHelper.RegisterUserAsync(client, "user1@email.com", "password", "password");
        await TestsHelper.RegisterUserAsync(client, "user2@email.com", "password", "password");
        await TestsHelper.LoginAsync(client, "user1@email.com", "password");

        //Act
        HttpResponseMessage response = await client.DeleteAsync(UriConstants.ADMIN_DELETE_USER_URI + "user2@email.com");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [TearDown]
    public async Task TearDownAsync()
    {
        client.Dispose();
        await application.DisposeAsync();
    }
}
