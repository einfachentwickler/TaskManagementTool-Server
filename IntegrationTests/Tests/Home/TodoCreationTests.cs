﻿using FluentAssertions;
using IntegrationTests.Constants;
using NUnit.Framework;
using System.Net.Http.Json;
using TaskManagementTool.BusinessLogic.ViewModels;
using TaskManagementTool.BusinessLogic.ViewModels.AuthModels;
using TaskManagementTool.BusinessLogic.ViewModels.ToDoModels;

namespace IntegrationTests.Tests.Home;

[TestFixture]
public class TodoCreationTests
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
        await RegisterAndLoginUserAsync();

        CreateTodoDto createTodoDto = new()
        {
            Name = "Todo 1",
            Content = "Content 1",
            Importance = 5
        };

        HttpResponseMessage createResponse = await _client.PostAsJsonAsync(UriConstants.CREATE_TODO_URI, createTodoDto);

        createResponse.EnsureSuccessStatusCode();

        TodoDto? todo = await createResponse.Content.ReadFromJsonAsync<TodoDto>();

        HttpResponseMessage getResponse = await _client.GetAsync(UriConstants.GET_TODO_URI + todo!.Id);

        getResponse.EnsureSuccessStatusCode();

        TodoDto? todoFromDb = await getResponse.Content.ReadFromJsonAsync<TodoDto>();

        todoFromDb!.Content.Should().Be(createTodoDto.Content);
        todoFromDb.Name.Should().Be(createTodoDto.Name);
        todoFromDb.Importance.Should().Be(createTodoDto.Importance);
        todoFromDb.Id.Should().Be(todo.Id);
    }

    [Test]
    public async Task UpdateTodo_ValidData_TodoIsUpdated()
    {
        //Arrange
        await RegisterAndLoginUserAsync();

        CreateTodoDto createTodoDto = new()
        {
            Name = "Todo 1",
            Content = "Content 1",
            Importance = 5
        };

        HttpResponseMessage createResponse = await _client.PostAsJsonAsync(UriConstants.CREATE_TODO_URI, createTodoDto);

        TodoDto? todo = await createResponse.Content.ReadFromJsonAsync<TodoDto>();

        UpdateTodoDto updateTodoDto = new()
        {
            Name = "Todo upd",
            Content = "Content upd",
            Importance = 10,
            Id = todo!.Id
        };

        //Act
        HttpResponseMessage updateResponse = await _client.PutAsJsonAsync(UriConstants.UPDATE_TODO_URI, updateTodoDto);

        //Assert
        updateResponse.EnsureSuccessStatusCode();

        HttpResponseMessage getResponse = await _client.GetAsync(UriConstants.GET_TODO_URI + todo.Id);

        getResponse.EnsureSuccessStatusCode();

        TodoDto? todoFromDb = await getResponse.Content.ReadFromJsonAsync<TodoDto>();

        todoFromDb!.Content.Should().Be(updateTodoDto.Content);
        todoFromDb.Name.Should().Be(updateTodoDto.Name);
        todoFromDb.Importance.Should().Be(updateTodoDto.Importance);
        todoFromDb.Id.Should().Be(todo.Id);
    }

    private async Task RegisterAndLoginUserAsync()
    {
        RegisterDto registerDto = new()
        {
            Age = 34,
            Password = "password",
            ConfirmPassword = "password",
            Email = "user1@email.com",
            FirstName = "First name",
            LastName = "Last name"
        };

        await _client.PostAsJsonAsync(UriConstants.REGISTER_URI, registerDto);

        LoginDto loginDto = new()
        {
            Email = registerDto.Email,
            Password = registerDto.Password
        };

        HttpResponseMessage loginResponse = await _client.PostAsJsonAsync(UriConstants.LOGIN_URI, loginDto);

        string token = (await loginResponse.Content.ReadFromJsonAsync<UserManagerResponse>())!.Message;

        _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
    }

    [TearDown]
    public async Task TearDownAsync()
    {
        _client.Dispose();
        await _application.DisposeAsync();
    }
}