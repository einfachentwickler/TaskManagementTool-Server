using Application.Commands.Home.CreateTodo.Models;
using Application.Commands.Home.UpdateTodo.Models;
using Application.Queries.Home.GetTodoById.Models;
using FluentAssertions;
using IntegrationTests.Constants;
using IntegrationTests.Utils;
using NUnit.Framework;
using System.Net.Http.Json;

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

        var createTodoDto = new CreateTodoCommand
        {
            Name = "Todo 1",
            Content = "Content 1",
            Importance = 5
        };

        var createResponse = await _client.PostAsJsonAsync(UriConstants.HOME_CREATE_TODO_URI, createTodoDto);

        var response = await createResponse.Content.ReadFromJsonAsync<CreateTodoResponse>();

        var updateTodoDto = new UpdateTodoCommand
        {
            Name = "Todo upd",
            Content = "Content upd",
            Importance = 10,
            Id = response!.Id,
            IsCompleted = false
        };

        //Act
        var updateResponse = await _client.PutAsJsonAsync(UriConstants.HOME_UPDATE_TODO_URI, updateTodoDto);

        //Assert
        updateResponse.EnsureSuccessStatusCode();

        var getResponse = await _client.GetAsync(string.Format(UriConstants.HOME_GET_TODO_URI, response.Id));

        getResponse.EnsureSuccessStatusCode();

        var todoFromDb = await getResponse.Content.ReadFromJsonAsync<GetTodoByIdResponse>();

        todoFromDb!.Content.Should().Be(updateTodoDto.Content);
        todoFromDb.Name.Should().Be(updateTodoDto.Name);
        todoFromDb.Importance.Should().Be(updateTodoDto.Importance);
        todoFromDb.Id.Should().Be(response.Id);
    }

    [TearDown]
    public async Task TearDownAsync()
    {
        _client.Dispose();
        await _application.DisposeAsync();
    }
}