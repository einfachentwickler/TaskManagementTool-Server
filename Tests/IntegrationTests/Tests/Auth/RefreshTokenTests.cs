using Application.Commands.Auth.Login.Models;
using Application.Commands.Auth.RefreshToken.Models;
using FluentAssertions;
using IntegrationTests.Constants;
using IntegrationTests.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System.Net;
using System.Net.Http.Json;

namespace IntegrationTests.Tests.Auth;

[TestFixture]
public class RefreshTokenTests
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
    public async Task Refresh_ValidScenario_Success()
    {
        //Arrange
        var loginResponse = await TestsHelper.LoginAsync(_client, "admin@example.com", "password");

        var refreshCommand = new RefreshTokenCommand { RefreshToken = loginResponse.RefreshToken };

        //Act
        var response = await _client.PostAsJsonAsync(UriConstants.AUTH_REFRESH_TOKEN, refreshCommand);

        //Arrange
        response.EnsureSuccessStatusCode();

        var actualResult = await response.Content.ReadFromJsonAsync<UserLoginResponse>();

        actualResult!.AccessToken.Should().NotBeEquivalentTo(loginResponse.AccessToken);
        actualResult.Expires.Should().BeAfter(DateTime.UtcNow);
        actualResult.RefreshToken.Should().NotBeEquivalentTo(loginResponse.RefreshToken);
    }

    [Test]
    public async Task Refresh_ReusesRefreshToken_Throws500()
    {
        //Arrange
        var loginResponse = await TestsHelper.LoginAsync(_client, "admin@example.com", "password");

        var refreshCommand = new RefreshTokenCommand { RefreshToken = loginResponse.RefreshToken };

        //Act
        var response = await _client.PostAsJsonAsync(UriConstants.AUTH_REFRESH_TOKEN, refreshCommand);

        //Arrange
        response.EnsureSuccessStatusCode();

        var response500 = await _client.PostAsJsonAsync(UriConstants.AUTH_REFRESH_TOKEN, refreshCommand);

        response500.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

        var actualResult = await response500.Content.ReadFromJsonAsync<ProblemDetails>();

        actualResult.Should().BeEquivalentTo(new ProblemDetails
        {
            Title = "Internal server error",
            Detail = "Unexpected error occured",
            Status = StatusCodes.Status500InternalServerError
        }, options => options.Excluding(dto => dto.Extensions));
    }

    [TearDown]
    public async Task TearDownAsync()
    {
        _client.Dispose();
        await _application.DisposeAsync();
    }
}