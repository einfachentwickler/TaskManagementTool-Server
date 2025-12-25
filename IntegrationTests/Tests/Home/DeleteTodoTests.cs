using Application.Commands.Home.CreateTodo.Models;
using Application.Dto.ToDoModels;
using FluentAssertions;
using IntegrationTests.Constants;
using IntegrationTests.Utils;
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

        CreateTodoDto createTodoDto = new()
        {
            Name = "Todo 1",
            Content = "Content 1",
            Importance = 5
        };

        CreateTodoDto createTodoDto2 = new()
        {
            Name = "Todo 2",
            Content = "Content 2",
            Importance = 7
        };

        HttpResponseMessage createResponse = await _client.PostAsJsonAsync(UriConstants.HOME_CREATE_TODO_URI, createTodoDto);
        HttpResponseMessage createResponse2 = await _client.PostAsJsonAsync(UriConstants.HOME_CREATE_TODO_URI, createTodoDto2);

        CreateTodoResponse? createdTodo = await createResponse.Content.ReadFromJsonAsync<CreateTodoResponse>();
        CreateTodoResponse? createdTodo2 = await createResponse2.Content.ReadFromJsonAsync<CreateTodoResponse>();

        //Act
        HttpResponseMessage deleteResponse = await _client.DeleteAsync(string.Format(UriConstants.HOME_DELETE_TODO_URI, createdTodo!.Todo.Id));

        //Assert
        deleteResponse.EnsureSuccessStatusCode();

        HttpResponseMessage deleteResponse2 = await _client.DeleteAsync(string.Format(UriConstants.HOME_DELETE_TODO_URI, createdTodo!.Todo.Id));
        deleteResponse2.StatusCode.Should().Be(HttpStatusCode.NotFound);

        HttpResponseMessage getResponse = await _client.GetAsync(string.Format(UriConstants.HOME_GET_TODO_URI, createdTodo2!.Todo.Id));
        getResponse.EnsureSuccessStatusCode();
    }

    [Test]
    public async Task DeleteTodo_ForbiddedTodo_ReturnsForbidden()
    {
        //Arrange
        await TestsHelper.RegisterUserAsync(_client, "user1@email.com", "password", "password");
        await TestsHelper.RegisterUserAsync(_client, "user2@email.com", "password", "password");
        await TestsHelper.LoginAsync(_client, "user1@email.com", "password");

        CreateTodoDto createTodoDto = new()
        {
            Name = "Todo 1",
            Content = "Content 1",
            Importance = 5
        };

        HttpResponseMessage createResponse = await _client.PostAsJsonAsync(UriConstants.HOME_CREATE_TODO_URI, createTodoDto);

        CreateTodoResponse? createdTodo = await createResponse.Content.ReadFromJsonAsync<CreateTodoResponse>();

        await TestsHelper.LoginAsync(_client, "user2@email.com", "password");

        //Act
        HttpResponseMessage deleteResponse = await _client.DeleteAsync(string.Format(UriConstants.HOME_DELETE_TODO_URI, createdTodo!.Todo.Id));

        //Assert
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);

        await TestsHelper.LoginAsync(_client, "user1@email.com", "password");

        HttpResponseMessage getTodoResponse = await _client.DeleteAsync(string.Format(UriConstants.HOME_GET_TODO_URI, createdTodo!.Todo.Id));
        getTodoResponse.EnsureSuccessStatusCode();
    }

    [Test]
    public async Task DeleteTodo_TodoDoesNotExist_ReturnsNotFound()
    {
        //Arrange
        await TestsHelper.RegisterUserAsync(_client, "user1@email.com", "password", "password");
        await TestsHelper.LoginAsync(_client, "user1@email.com", "password");

        //Act
        HttpResponseMessage deleteResponse = await _client.DeleteAsync(string.Format(UriConstants.HOME_DELETE_TODO_URI, 200));

        //Assert
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [TearDown]
    public async Task TearDownAsync()
    {
        _client.Dispose();
        await _application.DisposeAsync();
    }
}
