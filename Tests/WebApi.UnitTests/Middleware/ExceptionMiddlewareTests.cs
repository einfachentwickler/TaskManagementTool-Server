using Application.Queries.Home.GetTodoById.Models;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using FluentAssertions;
using LoggerService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NSubstitute;
using NUnit.Framework;
using Shared.Exceptions;
using WebApi.Middleware;
using WebApi.UnitTests.Utils;

namespace WebApi.UnitTests.Middleware;

[TestFixture]
public class ExceptionMiddlewareTests
{
    private IFixture _fixture;
    private ILoggerManager _loggerManager;

    [SetUp]
    public void Setup()
    {
        _fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        _loggerManager = _fixture.Freeze<ILoggerManager>();
    }

    [TestCase(GetTodoByIdErrorCode.TodoNotFound, StatusCodes.Status404NotFound)]
    [TestCase(GetTodoByIdErrorCode.Forbidden, StatusCodes.Status403Forbidden)]
    public async Task InvokeAsync_WhenCustomExceptionThrown_ReturnsExpectedProblemDetails(GetTodoByIdErrorCode errorCode, int expectedStatusCode)
    {
        // Arrange
        var exception = new CustomException<GetTodoByIdErrorCode>(errorCode, GetTodoByIdErrorMesssages.TodoNotFound);

        var middleware = new ExceptionMiddleware((_) => throw exception);

        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        // Act
        await middleware.InvokeAsync(context, _loggerManager);

        // Assert
        context.Response.StatusCode.Should().Be(expectedStatusCode);

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var body = await new StreamReader(context.Response.Body).ReadToEndAsync();

        var problem = JsonConvert.DeserializeObject<ProblemDetails>(body);

        problem.Should().BeEquivalentTo(new ProblemDetails
        {
            Title = errorCode.ToString(),
            Detail = GetTodoByIdErrorMesssages.TodoNotFound,
            Status = expectedStatusCode
        });

        _loggerManager.Received(1).LogError(ExtendedArg.Is(exception));
    }

    [Test]
    public async Task InvokeAsync_WhenUnhandledExceptionThrown_ReturnsInternalServerError()
    {
        // Arrange
        var exception = new Exception();

        var middleware = new ExceptionMiddleware((_) => throw exception);

        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        // Act
        await middleware.InvokeAsync(context, _loggerManager);

        // Assert
        context.Response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var body = await new StreamReader(context.Response.Body).ReadToEndAsync();

        var problem = JsonConvert.DeserializeObject<ProblemDetails>(body);

        problem.Should().BeEquivalentTo(new ProblemDetails
        {
            Title = "Internal server error",
            Detail = "Unexpected error occured",
            Status = StatusCodes.Status500InternalServerError
        });

        _loggerManager.Received(1).LogError(ExtendedArg.Is(exception));
    }
}