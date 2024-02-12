﻿using FluentAssertions;
using IntegrationTests.Constants;
using IntegrationTests.Utils;
using NUnit.Framework;
using System.Net.Http.Json;
using TaskManagementTool.BusinessLogic.ViewModels;
using TaskManagementTool.BusinessLogic.ViewModels.ToDoModels;

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

        TodoDto? todo = await createResponse.Content.ReadFromJsonAsync<TodoDto>();

        HttpResponseMessage getResponse = await _client.GetAsync(UriConstants.HOME_GET_TODO_URI + todo!.Id);

        getResponse.EnsureSuccessStatusCode();

        TodoDto? todoFromDb = await getResponse.Content.ReadFromJsonAsync<TodoDto>();

        todoFromDb!.Content.Should().Be(createTodoDto.Content);
        todoFromDb.Name.Should().Be(createTodoDto.Name);
        todoFromDb.Importance.Should().Be(createTodoDto.Importance);
        todoFromDb.Id.Should().Be(todo.Id);
    }

    [TearDown]
    public async Task TearDownAsync()
    {
        _client.Dispose();
        await _application.DisposeAsync();
    }
}