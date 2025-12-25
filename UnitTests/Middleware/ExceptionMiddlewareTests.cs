using AutoFixture;
using AutoFixture.AutoNSubstitute;
using FluentAssertions;
using LoggerService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NSubstitute;
using NUnit.Framework;
using TaskManagementTool.Common.Enums;
using TaskManagementTool.Common.Exceptions;
using TaskManagementTool.Host.Middleware;

namespace Host.UnitTests.Middleware;

[TestFixture]
public class ExceptionMiddlewareTests
{
    private IFixture fixture;
    private ILoggerManager loggerManager;

    [SetUp]
    public void Setup()
    {
        fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        loggerManager = fixture.Freeze<ILoggerManager>();
    }

    [TestCase(ApiErrorCode.Unautorized, StatusCodes.Status401Unauthorized)]
    [TestCase(ApiErrorCode.TodoNotFound, StatusCodes.Status404NotFound)]
    [TestCase(ApiErrorCode.UserNotFound, StatusCodes.Status404NotFound)]
    [TestCase(ApiErrorCode.InvalidInput, StatusCodes.Status400BadRequest)]
    [TestCase(ApiErrorCode.Forbidden, StatusCodes.Status404NotFound)]
    [TestCase((ApiErrorCode)349, StatusCodes.Status500InternalServerError)]
    public async Task Invoke_ThrowsTaskManagementToolException_ReturnsValidErrorData(ApiErrorCode apiErrorCode, int expectedStatusCode)
    {
        //Arrange
        string expectedContent = JsonConvert.SerializeObject(new ProblemDetails { Title = apiErrorCode.ToString(), Detail = "message" });

        CustomException expectedException = new(apiErrorCode, "message");

        ExceptionMiddleware sut = new(next: (_) => throw expectedException);

        DefaultHttpContext context = new();
        context.Response.Body = new MemoryStream();

        //Act
        await sut.InvokeAsync(context, loggerManager);

        context.Response.Body.Seek(0, SeekOrigin.Begin);

        //Assert
        context.Response.StatusCode.Should().Be(expectedStatusCode);

        string serializedResponse = await new StreamReader(context.Response.Body).ReadToEndAsync();

        serializedResponse.Should().Be(expectedContent);

        loggerManager.Received(1).LogError(expectedException);
    }

    [Test]
    public async Task Invoke_ThrowsUnknownException_ReturnsGeneric500()
    {
        //Arrange
        NotImplementedException expectedException = new();

        ExceptionMiddleware sut = new(next: (_) => throw expectedException);

        DefaultHttpContext context = new();
        context.Response.Body = new MemoryStream();

        //Act
        await sut.InvokeAsync(context, loggerManager);

        context.Response.Body.Seek(0, SeekOrigin.Begin);

        //Assert
        context.Response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);

        string serializedResponse = await new StreamReader(context.Response.Body).ReadToEndAsync();

        serializedResponse.Should().Be("Internal server error");

        loggerManager.Received(1).LogError(expectedException);
    }

    [Test]
    public async Task Invoke_DoesNotThrow_Returns200()
    {
        //Arrange
        ExceptionMiddleware sut = new(next: async (context) => { context.Response.StatusCode = StatusCodes.Status200OK; await Task.FromResult(context); });

        DefaultHttpContext context = new();
        context.Response.Body = new MemoryStream();

        //Act
        await sut.InvokeAsync(context, loggerManager);

        context.Response.Body.Seek(0, SeekOrigin.Begin);

        //Assert
        context.Response.StatusCode.Should().Be(StatusCodes.Status200OK);

        string serializedResponse = await new StreamReader(context.Response.Body).ReadToEndAsync();

        serializedResponse.Should().Be(string.Empty);

        loggerManager.DidNotReceiveWithAnyArgs().LogError(Arg.Any<Exception>());
    }
}