using Application.Queries.Admin.GetUsers.Models;
using FluentAssertions;
using IntegrationTests.Constants;
using IntegrationTests.Utils;
using NUnit.Framework;
using System.Net;
using System.Net.Http.Json;

namespace IntegrationTests.Tests.Admin;

[TestFixture]
public class GetUsersTests
{
    private TmtWebApplicationFactory _application;
    private HttpClient _client;

    [SetUp]
    public void Setup()
    {
        _application = new TmtWebApplicationFactory();
        _client = _application.CreateClient();
    }

    [TestCase(1, 5, 5)]
    [TestCase(2, 5, 5)]
    [TestCase(1, 10, 10)]
    [TestCase(2, 10, 6)]
    [TestCase(10000000, 10, 10)]
    [TestCase(1, 10000000, 10)]
    public async Task GetUsers_Page_ReturnsUsers(int pageNumber, int pageSize, int expectedSize)
    {
        //Arrange
        await TestsHelper.RegisterUserAsync(_client, "user1@email.com", "password", "password");
        await TestsHelper.RegisterUserAsync(_client, "user2@email.com", "password", "password");
        await TestsHelper.RegisterUserAsync(_client, "user3@email.com", "password", "password");
        await TestsHelper.RegisterUserAsync(_client, "user4@email.com", "password", "password");
        await TestsHelper.RegisterUserAsync(_client, "user5@email.com", "password", "password");
        await TestsHelper.RegisterUserAsync(_client, "user6@email.com", "password", "password");
        await TestsHelper.RegisterUserAsync(_client, "user7@email.com", "password", "password");
        await TestsHelper.RegisterUserAsync(_client, "user8@email.com", "password", "password");
        await TestsHelper.RegisterUserAsync(_client, "user9@email.com", "password", "password");
        await TestsHelper.RegisterUserAsync(_client, "user10@email.com", "password", "password");
        await TestsHelper.RegisterUserAsync(_client, "user11@email.com", "password", "password");
        await TestsHelper.RegisterUserAsync(_client, "user12@email.com", "password", "password");
        await TestsHelper.RegisterUserAsync(_client, "user13@email.com", "password", "password");
        await TestsHelper.RegisterUserAsync(_client, "user14@email.com", "password", "password");
        await TestsHelper.RegisterUserAsync(_client, "user15@email.com", "password", "password");

        await TestsHelper.LoginAsync(_client, "admin@example.com", "password");

        //Act
        var response = await _client.GetAsync(string.Format(UriConstants.ADMIN_GET_USERS_URI, pageSize, pageNumber));

        //Assert
        response.EnsureSuccessStatusCode();

        var actualResult = await response.Content.ReadFromJsonAsync<GetUsersResponse>();

        actualResult!.Users.Should().HaveCount(expectedSize);
        actualResult.Users.Should().AllSatisfy(x => x.Should().NotBeNull());
    }

    [Test]
    public async Task GetUsers_NotAdmin_Forbidden()
    {
        //Arrange
        await TestsHelper.RegisterUserAsync(_client, "user1@email.com", "password", "password");
        await TestsHelper.LoginAsync(_client, "user1@email.com", "password");

        //Act
        HttpResponseMessage response = await _client.GetAsync(UriConstants.ADMIN_GET_USERS_URI + "?pageNumber=1&pageSize=10");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [TearDown]
    public async Task TearDownAsync()
    {
        _client.Dispose();
        await _application.DisposeAsync();
    }
}