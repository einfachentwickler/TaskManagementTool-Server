using FluentAssertions;
using IntegrationTests.Constants;
using IntegrationTests.Utils;
using NUnit.Framework;
using System.Net;
using System.Net.Http.Json;
using TaskManagementTool.BusinessLogic.ViewModels;
using TaskManagementTool.BusinessLogic.ViewModels.ToDoModels;

namespace IntegrationTests.Tests.Home;

[TestFixture]
public class TodoDeletionTests
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

        HttpResponseMessage createResponse = await _client.PostAsJsonAsync(UriConstants.CREATE_TODO_URI, createTodoDto);

        TodoDto? createdTodo = await createResponse.Content.ReadFromJsonAsync<TodoDto>();

        //Act
        HttpResponseMessage deleteResponse = await _client.DeleteAsync(UriConstants.DELETE_TODO_URI + createdTodo!.Id);

        //Assert
        deleteResponse.EnsureSuccessStatusCode();

        HttpResponseMessage deleteResponse2 = await _client.DeleteAsync(UriConstants.DELETE_TODO_URI + createdTodo!.Id);
        deleteResponse2.StatusCode.Should().Be(HttpStatusCode.NotFound);
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

        HttpResponseMessage createResponse = await _client.PostAsJsonAsync(UriConstants.CREATE_TODO_URI, createTodoDto);

        TodoDto? createdTodo = await createResponse.Content.ReadFromJsonAsync<TodoDto>();

        await TestsHelper.LoginAsync(_client, "user2@email.com", "password");

        //Act
        HttpResponseMessage deleteResponse = await _client.DeleteAsync(UriConstants.DELETE_TODO_URI + createdTodo!.Id);

        //Assert
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.Forbidden);

        await TestsHelper.LoginAsync(_client, "user1@email.com", "password");
        HttpResponseMessage getTodoResponse = await _client.DeleteAsync(UriConstants.GET_TODO_URI + createdTodo!.Id);
        getTodoResponse.EnsureSuccessStatusCode();
    }

    [Test]
    public async Task DeleteTodo_TodoDoesNotExist_ReturnsNotFound()
    {
        //Arrange
        await TestsHelper.RegisterUserAsync(_client, "user1@email.com", "password", "password");
        await TestsHelper.LoginAsync(_client, "user1@email.com", "password");

        //Act
        HttpResponseMessage deleteResponse = await _client.DeleteAsync(UriConstants.DELETE_TODO_URI + 200);

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
