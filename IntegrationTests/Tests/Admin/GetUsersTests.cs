using FluentAssertions;
using IntegrationTests.Constants;
using IntegrationTests.Utils;
using NUnit.Framework;
using System.Net;
using System.Net.Http.Json;
using TaskManagementTool.BusinessLogic.ViewModels;

namespace IntegrationTests.Tests.Admin;

[TestFixture]
public class GetUsersTests
{
    private TmtWebApplicationFactory application;
    private HttpClient client;

    [SetUp]
    public void Setup()
    {
        application = new TmtWebApplicationFactory();
        client = application.CreateClient();
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
        await TestsHelper.RegisterUserAsync(client, "user1@email.com", "password", "password");
        await TestsHelper.RegisterUserAsync(client, "user2@email.com", "password", "password");
        await TestsHelper.RegisterUserAsync(client, "user3@email.com", "password", "password");
        await TestsHelper.RegisterUserAsync(client, "user4@email.com", "password", "password");
        await TestsHelper.RegisterUserAsync(client, "user5@email.com", "password", "password");
        await TestsHelper.RegisterUserAsync(client, "user6@email.com", "password", "password");
        await TestsHelper.RegisterUserAsync(client, "user7@email.com", "password", "password");
        await TestsHelper.RegisterUserAsync(client, "user8@email.com", "password", "password");
        await TestsHelper.RegisterUserAsync(client, "user9@email.com", "password", "password");
        await TestsHelper.RegisterUserAsync(client, "user10@email.com", "password", "password");
        await TestsHelper.RegisterUserAsync(client, "user11@email.com", "password", "password");
        await TestsHelper.RegisterUserAsync(client, "user12@email.com", "password", "password");
        await TestsHelper.RegisterUserAsync(client, "user13@email.com", "password", "password");
        await TestsHelper.RegisterUserAsync(client, "user14@email.com", "password", "password");
        await TestsHelper.RegisterUserAsync(client, "user15@email.com", "password", "password");

        await TestsHelper.LoginAsync(client, "admin@example.com", "password");

        //Act
        HttpResponseMessage response = await client.GetAsync(UriConstants.ADMIN_GET_USERS_URI + $"?pageNumber={pageNumber}&pageSize={pageSize}");

        //Assert
        response.EnsureSuccessStatusCode();

        var actualResult = await response.Content.ReadFromJsonAsync<IEnumerable<UserDto>>();

        actualResult.Should().HaveCount(expectedSize);
        actualResult.Should().AllSatisfy(x => x.Should().NotBeNull());
    }

    [Test]
    public async Task GetUsers_NotAdmin_Forbidden()
    {
        //Arrange
        await TestsHelper.RegisterUserAsync(client, "user1@email.com", "password", "password");
        await TestsHelper.LoginAsync(client, "user1@email.com", "password");

        //Act
        HttpResponseMessage response = await client.GetAsync(UriConstants.ADMIN_GET_USERS_URI + "?pageNumber=1&pageSize=10");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [TearDown]
    public async Task TearDownAsync()
    {
        client.Dispose();
        await application.DisposeAsync();
    }
}