using Application.Commands.Home.CreateTodo.Models;
using FluentAssertions;
using IntegrationTests.Constants;
using IntegrationTests.Utils;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using System.Net;
using System.Net.Http.Json;

namespace IntegrationTests.Tests.Home;

[TestFixture]
public class DeleteTodoTests
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
    public async Task DeleteTodo_ValidPath_TodoIsDeleted()
    {
        //Arrange
        await TestsHelper.RegisterUserAsync(_client, "user1@email.com", "password", "password");
        await TestsHelper.LoginAsync(_client, "user1@email.com", "password");

        var createTodoDto = new CreateTodoCommand
        {
            Name = "Todo 1",
            Content = "Content 1",
            Importance = 5
        };

        var createTodoDto2 = new CreateTodoCommand
        {
            Name = "Todo 2",
            Content = "Content 2",
            Importance = 7
        };

        var createResponse = await _client.PostAsJsonAsync(UriConstants.HOME_CREATE_TODO_URI, createTodoDto);
        var createResponse2 = await _client.PostAsJsonAsync(UriConstants.HOME_CREATE_TODO_URI, createTodoDto2);

        var createdTodo = await createResponse.Content.ReadFromJsonAsync<CreateTodoResponse>();
        var createdTodo2 = await createResponse2.Content.ReadFromJsonAsync<CreateTodoResponse>();

        //Act
        var deleteResponse = await _client.DeleteAsync(string.Format(UriConstants.HOME_DELETE_TODO_URI, createdTodo!.Todo.Id));

        //Assert
        deleteResponse.EnsureSuccessStatusCode();

        var getResponse1 = await _client.GetAsync(string.Format(UriConstants.HOME_GET_TODO_URI, createdTodo!.Todo.Id));
        getResponse1.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var getResponse = await _client.GetAsync(string.Format(UriConstants.HOME_GET_TODO_URI, createdTodo2!.Todo.Id));
        getResponse.EnsureSuccessStatusCode();
    }

    [Test]
    public async Task DeleteTodo_ForbiddedTodo_ReturnsForbidden()
    {
        //Arrange
        await TestsHelper.RegisterUserAsync(_client, "user1@email.com", "password", "password");
        await TestsHelper.RegisterUserAsync(_client, "user2@email.com", "password", "password");
        await TestsHelper.LoginAsync(_client, "user1@email.com", "password");

        var createTodoDto = new CreateTodoCommand
        {
            Name = "Todo 1",
            Content = "Content 1",
            Importance = 5
        };

        var createResponse = await _client.PostAsJsonAsync(UriConstants.HOME_CREATE_TODO_URI, createTodoDto);

        var createdTodo = await createResponse.Content.ReadFromJsonAsync<CreateTodoResponse>();

        await TestsHelper.LoginAsync(_client, "user2@email.com", "password");

        //Act
        var deleteResponse = await _client.DeleteAsync(string.Format(UriConstants.HOME_DELETE_TODO_URI, createdTodo!.Todo.Id));

        //Assert
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.Forbidden);

        await TestsHelper.LoginAsync(_client, "user1@email.com", "password");

        var getTodoResponse = await _client.DeleteAsync(string.Format(UriConstants.HOME_GET_TODO_URI, createdTodo!.Todo.Id));
        getTodoResponse.EnsureSuccessStatusCode();
    }

    [TearDown]
    public async Task TearDownAsync()
    {
        _client.Dispose();
        await _application.DisposeAsync();
    }
}
