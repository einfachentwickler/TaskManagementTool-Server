using Application.Queries.Home.GetTodoById.Models;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NUnit.Framework;
using Shared.Exceptions;
using WebApi.Middleware;

namespace WebApi.UnitTests.Middleware;

[TestFixture]
public class ExceptionMiddlewareTests
{
    private IFixture _fixture;
    private ILogger<ExceptionMiddleware> _logger;

    [SetUp]
    public void Setup()
    {
        _fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        _logger = _fixture.Freeze<ILogger<ExceptionMiddleware>>();
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
        await middleware.InvokeAsync(context, _logger);

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
        await middleware.InvokeAsync(context, _logger);

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
    }
}