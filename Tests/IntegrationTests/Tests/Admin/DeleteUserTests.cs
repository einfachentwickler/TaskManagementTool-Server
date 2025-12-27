using Application.Commands.Admin.DeleteUser.Models;
using Application.Queries.Admin.GetTodos.Models;
using Application.Queries.Admin.GetUsers.Models;
using FluentAssertions;
using IntegrationTests.Constants;
using IntegrationTests.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System.Net;
using System.Net.Http.Json;

namespace IntegrationTests.Tests.Admin;

[TestFixture]
public class DeleteUserTests
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
    public async Task DeleteUser_ValidPath_UserAndHisTodosAreDeleted()
    {
        //Arrange
        await TestsHelper.RegisterUserAsync(_client, "user1@email.com", "password", "password");
        await TestsHelper.RegisterUserAsync(_client, "user2@email.com", "password", "password");

        await TestsHelper.LoginAsync(_client, "user1@email.com", "password");
        await TestsHelper.CreateTodoAsync(_client, "Todo 1");
        await TestsHelper.CreateTodoAsync(_client, "Todo 2");
        await TestsHelper.CreateTodoAsync(_client, "Todo 3");

        await TestsHelper.LoginAsync(_client, "user2@email.com", "password");
        await TestsHelper.CreateTodoAsync(_client, "Todo 4");
        await TestsHelper.CreateTodoAsync(_client, "Todo 5");
        await TestsHelper.CreateTodoAsync(_client, "Todo 6");

        await TestsHelper.LoginAsync(_client, "admin@example.com", "password");

        //Act
        var response = await _client.DeleteAsync(UriConstants.ADMIN_DELETE_USER_URI + "user2@email.com");

        //Assert
        response.EnsureSuccessStatusCode();

        var getUsersResponse = await _client.GetAsync(string.Format(UriConstants.ADMIN_GET_USERS_URI, 10, 1));

        var users = await getUsersResponse.Content.ReadFromJsonAsync<GetUsersResponse>();

        users!.Users.Should().HaveCount(2)
            .And.Contain(x => x.Email == "admin@example.com")
            .And.Contain(x => x.Email == "user1@email.com");

        var getTodosResponse = await _client.GetAsync(string.Format(UriConstants.ADMIN_GET_TODOS_URI, 10, 1));

        var todos = await getTodosResponse.Content.ReadFromJsonAsync<GetTodosByAdminResponse>();
        todos!.Todos.Should().HaveCount(5)
            .And.Contain(dto => dto.Name == "Todo 1")
            .And.Contain(dto => dto.Name == "Todo 2")
            .And.Contain(dto => dto.Name == "Todo 3");
    }

    [Test]
    public async Task DeleteUser_UserDoesNotExist_ReturnsNotFound()
    {
        //Arrange
        await TestsHelper.LoginAsync(_client, "admin@example.com", "password");

        //Act
        var response = await _client.DeleteAsync(UriConstants.ADMIN_DELETE_USER_URI + "user2@email.com");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var actualResult = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        actualResult.Should().BeEquivalentTo(new ProblemDetails
        {
            Title = nameof(DeleteUserErrorCode.UserNotFound),
            Status = StatusCodes.Status404NotFound,
            Detail = DeleteUserErrorMessages.UserNotFound
        }, options => options.Excluding(dto => dto.Extensions));
    }

    [Test]
    public async Task DeleteUser_NoPermission_ReturnsForbidden()
    {
        //Arrange
        await TestsHelper.RegisterUserAsync(_client, "user1@email.com", "password", "password");
        await TestsHelper.RegisterUserAsync(_client, "user2@email.com", "password", "password");
        await TestsHelper.LoginAsync(_client, "user1@email.com", "password");

        //Act
        HttpResponseMessage response = await _client.DeleteAsync(UriConstants.ADMIN_DELETE_USER_URI + "user2@email.com");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [TearDown]
    public async Task TearDownAsync()
    {
        _client.Dispose();
        await _application.DisposeAsync();
    }
}