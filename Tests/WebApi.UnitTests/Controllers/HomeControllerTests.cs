using Application.Commands.Home.CreateTodo.Models;
using Application.Commands.Home.DeleteTodo.Models;
using Application.Commands.Home.UpdateTodo.Models;
using Application.Queries.Home.GetTodoById.Models;
using Application.Queries.Home.GetTodos.Models;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using WebApi.Controllers;
using WebApi.UnitTests.Utils;

namespace WebApi.UnitTests.Controllers;

[TestFixture]
public class HomeControllerTests
{
    private IFixture _fixture;
    private IMediator _mediator;
    private CancellationToken _cancellationToken;

    private HomeController _sut;

    [SetUp]
    public void Setup()
    {
        _fixture = new Fixture().Customize(new AutoNSubstituteCustomization());

        _mediator = _fixture.Freeze<IMediator>();

        _cancellationToken = _fixture.Create<CancellationToken>();

        _sut = new HomeController(_mediator);
    }

    [Test]
    public async Task GetTodos_ValidRequest_ReturnsOkWithList()
    {
        // Arrange
        var query = new GetTodosQuery
        {
            PageNumber = 1,
            PageSize = 10
        };

        var response = _fixture.Create<GetTodosResponse>();

        _mediator.Send(ExtendedArg.Is(query), _cancellationToken).Returns(response);

        // Act
        var actualResult = await _sut.GetTodos(query.PageNumber, query.PageSize, _cancellationToken);

        // Assert
        actualResult.Should().BeOfType<OkObjectResult>();
        actualResult.As<OkObjectResult>().Value.Should().BeEquivalentTo(response);
    }

    [Test]
    public async Task GetById_ValidId_ReturnsOkWithTodo()
    {
        // Arrange
        var query = new GetTodoByIdQuery
        {
            TodoId = 1
        };

        var response = _fixture.Create<GetTodoByIdResponse>();

        _mediator.Send(ExtendedArg.Is(query), _cancellationToken).Returns(response);

        // Act
        var actualResult = await _sut.GetById(query.TodoId, _cancellationToken);

        // Assert
        actualResult.Should().BeOfType<OkObjectResult>();
        actualResult.As<OkObjectResult>().Value.Should().BeEquivalentTo(response);
    }

    [Test]
    public async Task Create_ValidModel_ReturnsCreatedAtAction()
    {
        // Arrange
        var command = _fixture.Create<CreateTodoCommand>();

        var response = _fixture.Create<CreateTodoResponse>();

        _mediator.Send(ExtendedArg.Is(command), _cancellationToken).Returns(response);

        // Act
        var actualResult = await _sut.Create(command, _cancellationToken);

        // Assert
        actualResult.Should().BeOfType<CreatedAtActionResult>();
        var createdResult = actualResult.As<CreatedAtActionResult>();
        createdResult.Value.Should().BeEquivalentTo(response);
        createdResult.RouteValues!["id"].Should().Be(response.Todo.Id);
    }

    [Test]
    public async Task Update_ValidModel_ReturnsNoContent()
    {
        // Arrange
        var command = _fixture.Create<UpdateTodoCommand>();

        // Act
        var actualResult = await _sut.Update(command, _cancellationToken);

        // Assert
        actualResult.Should().BeOfType<NoContentResult>();

        await _mediator.Received(1).Send(ExtendedArg.Is(command), _cancellationToken);
    }

    [Test]
    public async Task Delete_ValidId_ReturnsNoContent()
    {
        // Arrange
        var command = new DeleteTodoCommand
        {
            TodoId = 12
        };

        // Act
        var actualResult = await _sut.Delete(command.TodoId, _cancellationToken);

        // Assert
        actualResult.Should().BeOfType<NoContentResult>();

        await _mediator.Received(1).Send(ExtendedArg.Is(command), _cancellationToken);
    }
}