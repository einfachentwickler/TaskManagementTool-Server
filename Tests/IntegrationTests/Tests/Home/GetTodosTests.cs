using Application.Commands.Home.CreateTodo.Models;
using Application.Queries.Home.GetTodos.Models;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using FluentAssertions;
using IntegrationTests.Constants;
using IntegrationTests.Utils;
using NUnit.Framework;
using System.Net;
using System.Net.Http.Json;

namespace IntegrationTests.Tests.Home;

[TestFixture]
public class GetTodosTests
{
    private TmtWebApplicationFactory _application;
    private HttpClient client;

    private IFixture fixture;

    [SetUp]
    public void Setup()
    {
        fixture = new Fixture().Customize(new AutoNSubstituteCustomization());

        _application = new TmtWebApplicationFactory();
        client = _application.CreateClient();
    }

    [Test]
    public async Task Get_ValidPath_ReturnsTodos()
    {
        //Arrange
        await TestsHelper.RegisterUserAsync(client, "user1@email.com", "password", "password");
        await TestsHelper.LoginAsync(client, "user1@email.com", "password");

        var todos = fixture.CreateMany<CreateTodoDto>(30).ToList();

        todos.ForEach(async todo => await client.PostAsJsonAsync(UriConstants.HOME_CREATE_TODO_URI, todo));

        //Act
        var response1 = await client.GetAsync(string.Format(UriConstants.HOME_GET_USER_TODOS, 10, 1));
        var response2 = await client.GetAsync(string.Format(UriConstants.HOME_GET_USER_TODOS, 10, 2));
        var response3 = await client.GetAsync(string.Format(UriConstants.HOME_GET_USER_TODOS, 10, 3));

        //Assert
        response1.EnsureSuccessStatusCode();
        response2.EnsureSuccessStatusCode();
        response3.EnsureSuccessStatusCode();

        var actualResult1 = await response1.Content.ReadFromJsonAsync<GetTodosResponse>();
        var actualResult2 = await response2.Content.ReadFromJsonAsync<GetTodosResponse>();
        var actualResult3 = await response3.Content.ReadFromJsonAsync<GetTodosResponse>();

        actualResult1!.Todos.Should().HaveCount(10);
        actualResult2!.Todos.Should().HaveCount(10);
        actualResult3!.Todos.Should().HaveCount(10);
    }

    [Test]
    public async Task Get_SomeoneElseTodo_ReturnsEmpty()
    {
        //Arrange
        await TestsHelper.RegisterUserAsync(client, "user1@email.com", "password", "password");
        await TestsHelper.RegisterUserAsync(client, "user2@email.com", "password", "password");
        await TestsHelper.LoginAsync(client, "user1@email.com", "password");

        var todos = fixture.CreateMany<CreateTodoDto>(30).ToList();

        todos.ForEach(async todo => await client.PostAsJsonAsync(UriConstants.HOME_CREATE_TODO_URI, todo));

        await TestsHelper.LoginAsync(client, "user2@email.com", "password");

        //Act
        var response1 = await client.GetAsync(string.Format(UriConstants.HOME_GET_USER_TODOS, 10, 1));
        var response2 = await client.GetAsync(string.Format(UriConstants.HOME_GET_USER_TODOS, 10, 2));
        var response3 = await client.GetAsync(string.Format(UriConstants.HOME_GET_USER_TODOS, 10, 3));

        //Assert
        response1.EnsureSuccessStatusCode();
        response2.EnsureSuccessStatusCode();
        response3.EnsureSuccessStatusCode();

        var actualResult1 = await response1.Content.ReadFromJsonAsync<GetTodosResponse>();
        var actualResult2 = await response2.Content.ReadFromJsonAsync<GetTodosResponse>();
        var actualResult3 = await response3.Content.ReadFromJsonAsync<GetTodosResponse>();

        actualResult1!.Todos.Should().BeNullOrEmpty();
        actualResult1.Todos.Should().BeNullOrEmpty();
        actualResult1.Todos.Should().BeNullOrEmpty();
    }

    [TearDown]
    public async Task TearDownAsync()
    {
        client.Dispose();
        await _application.DisposeAsync();
    }
}