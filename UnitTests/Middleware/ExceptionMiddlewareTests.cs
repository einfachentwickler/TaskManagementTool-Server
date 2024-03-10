using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using NUnit.Framework;
using TaskManagementTool.BusinessLogic.Dto.Errors;
using TaskManagementTool.Common.Enums;
using TaskManagementTool.Common.Exceptions;
using TaskManagementTool.Host.Middleware;

namespace Host.UnitTests.Middleware;

[TestFixture]
public class ExceptionMiddlewareTests
{
    [TestCase(ApiErrorCode.Unautorized, StatusCodes.Status401Unauthorized)]
    [TestCase(ApiErrorCode.TodoNotFound, StatusCodes.Status404NotFound)]
    [TestCase(ApiErrorCode.UserNotFound, StatusCodes.Status404NotFound)]
    [TestCase(ApiErrorCode.InvalidInput, StatusCodes.Status400BadRequest)]
    [TestCase(ApiErrorCode.Forbidden, StatusCodes.Status404NotFound)]
    [TestCase((ApiErrorCode)349, StatusCodes.Status500InternalServerError)]
    public async Task Invoke_ThrowsTaskManagementToolException_ReturnsValidErrorData(ApiErrorCode apiErrorCode, int expectedStatusCode)
    {
        //Arrange
        string expectedContent = JsonConvert.SerializeObject(new ErrorDto(apiErrorCode, "message"));

        ExceptionMiddleware sut = new(next: (_) => throw new TaskManagementToolException(apiErrorCode, "message"));

        DefaultHttpContext context = new();
        context.Response.Body = new MemoryStream();

        //Act
        await sut.InvokeAsync(context);

        context.Response.Body.Seek(0, SeekOrigin.Begin);

        //Assert
        context.Response.StatusCode.Should().Be(expectedStatusCode);

        string serializedResponse = await new StreamReader(context.Response.Body).ReadToEndAsync();

        serializedResponse.Should().Be(expectedContent);
    }

    [Test]
    public async Task Invoke_ThrowsUnknownException_ReturnsGeneric500()
    {
        //Arrange
        ExceptionMiddleware sut = new(next: (_) => throw new NotImplementedException());

        DefaultHttpContext context = new();
        context.Response.Body = new MemoryStream();

        //Act
        await sut.InvokeAsync(context);

        context.Response.Body.Seek(0, SeekOrigin.Begin);

        //Assert
        context.Response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);

        string serializedResponse = await new StreamReader(context.Response.Body).ReadToEndAsync();

        serializedResponse.Should().Be("Internal server error");
    }

    [Test]
    public async Task Invoke_DoesNotThrow_Returns200()
    {
        //Arrange
        ExceptionMiddleware sut = new(next: async (context) => { context.Response.StatusCode = StatusCodes.Status200OK; await Task.FromResult(context); });

        DefaultHttpContext context = new();
        context.Response.Body = new MemoryStream();

        //Act
        await sut.InvokeAsync(context);

        context.Response.Body.Seek(0, SeekOrigin.Begin);

        //Assert
        context.Response.StatusCode.Should().Be(StatusCodes.Status200OK);

        string serializedResponse = await new StreamReader(context.Response.Body).ReadToEndAsync();

        serializedResponse.Should().Be(string.Empty);
    }
}