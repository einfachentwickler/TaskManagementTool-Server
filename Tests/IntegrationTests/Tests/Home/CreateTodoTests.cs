using Application.Commands.Home.CreateTodo.Models;
using Application.Queries.Home.GetTodoById.Models;
using FluentAssertions;
using IntegrationTests.Constants;
using IntegrationTests.Utils;
using NUnit.Framework;
using System.Net.Http.Json;

namespace IntegrationTests.Tests.Home;

[TestFixture]
public class CreateTodoTests
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
    public async Task CreateTodo_ValidData_TodoIsCreated()
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

        HttpResponseMessage createResponse = await _client.PostAsJsonAsync(UriConstants.HOME_CREATE_TODO_URI, createTodoDto);

        createResponse.EnsureSuccessStatusCode();

        CreateTodoResponse? response = await createResponse.Content.ReadFromJsonAsync<CreateTodoResponse>();

        HttpResponseMessage getResponse = await _client.GetAsync(string.Format(UriConstants.HOME_GET_TODO_URI, response!.Todo.Id));

        getResponse.EnsureSuccessStatusCode();

        GetTodoByIdResponse? todoFromDb = await getResponse.Content.ReadFromJsonAsync<GetTodoByIdResponse>();

        todoFromDb!.Todo.Content.Should().Be(createTodoDto.Content);
        todoFromDb.Todo.Name.Should().Be(createTodoDto.Name);
        todoFromDb.Todo.Importance.Should().Be(createTodoDto.Importance);
        todoFromDb.Todo.Id.Should().Be(response.Todo.Id);
    }

    [TearDown]
    public async Task TearDownAsync()
    {
        _client.Dispose();
        await _application.DisposeAsync();
    }
}
