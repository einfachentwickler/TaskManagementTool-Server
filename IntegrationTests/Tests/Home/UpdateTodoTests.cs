using FluentAssertions;
using IntegrationTests.Constants;
using IntegrationTests.Utils;
using NUnit.Framework;
using System.Net.Http.Json;
using TaskManagementTool.BusinessLogic.Commands.Home.CreateTodo.Models;
using TaskManagementTool.BusinessLogic.Commands.Home.GetTodoById.Models;
using TaskManagementTool.BusinessLogic.Dto.ToDoModels;

namespace IntegrationTests.Tests.Home;

[TestFixture]
public class UpdateTodoTests
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
    public async Task UpdateTodo_ValidData_TodoIsUpdated()
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

        CreateTodoResponse? response = await createResponse.Content.ReadFromJsonAsync<CreateTodoResponse>();

        UpdateTodoDto updateTodoDto = new()
        {
            Name = "Todo upd",
            Content = "Content upd",
            Importance = 10,
            Id = response!.Todo.Id
        };

        //Act
        HttpResponseMessage updateResponse = await _client.PutAsJsonAsync(UriConstants.HOME_UPDATE_TODO_URI, updateTodoDto);

        //Assert
        updateResponse.EnsureSuccessStatusCode();

        HttpResponseMessage getResponse = await _client.GetAsync(string.Format(UriConstants.HOME_GET_TODO_URI, response.Todo.Id));

        getResponse.EnsureSuccessStatusCode();

        GetTodoByIdResponse? todoFromDb = await getResponse.Content.ReadFromJsonAsync<GetTodoByIdResponse>();

        todoFromDb!.Todo.Content.Should().Be(updateTodoDto.Content);
        todoFromDb.Todo.Name.Should().Be(updateTodoDto.Name);
        todoFromDb.Todo.Importance.Should().Be(updateTodoDto.Importance);
        todoFromDb.Todo.Id.Should().Be(response.Todo.Id);
    }

    [TearDown]
    public async Task TearDownAsync()
    {
        _client.Dispose();
        await _application.DisposeAsync();
    }
}