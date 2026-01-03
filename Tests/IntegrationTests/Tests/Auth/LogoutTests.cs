using Application.Commands.Auth.Logout.Models;
using Application.Commands.Home.CreateTodo.Models;
using Azure;
using Infrastructure.Context;
using IntegrationTests.Constants;
using IntegrationTests.Utils;
using NUnit.Framework;
using System.Net.Http.Json;

namespace IntegrationTests.Tests.Auth;

[TestFixture]
public class LogoutTests
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
    public async Task Logout_ValidData_LoggedOut()
    {
        //Arrange
        await TestsHelper.RegisterUserAsync(_client, "user1@email.com", "password", "password");
        var loginResponse = await TestsHelper.LoginAsync(_client, "user1@email.com", "password");

        //Act
        var logoutResponse = await _client.PostAsJsonAsync(UriConstants.AUTH_LOGOUT, new LogoutCommand(loginResponse.RefreshToken));

        //Assert
        logoutResponse.EnsureSuccessStatusCode();
    }

    [TearDown]
    public async Task TearDownAsync()
    {
        _client.Dispose();
        await _application.DisposeAsync();
    }
}