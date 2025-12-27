using Application.Commands.Admin.DeleteUser.Models;
using Application.Commands.Admin.ReverseStatus.Models;
using Application.Queries.Admin.GetTodos.Models;
using Application.Queries.Admin.GetUsers.Models;
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
public class AdminControllerTests
{
    private IFixture _fixture;
    private IMediator _mediator;
    private CancellationToken _cancellationToken;

    private AdminController _sut;

    [SetUp]
    public void Setup()
    {
        _fixture = new Fixture().Customize(new AutoNSubstituteCustomization());

        _mediator = _fixture.Freeze<IMediator>();
        _cancellationToken = _fixture.Create<CancellationToken>();

        _sut = new AdminController(_mediator);
    }

    [Test]
    public async Task GetUsers_Paging_ReturnsOk()
    {
        //Arrange
        var request = _fixture.Create<GetUsersQuery>();
        var response = _fixture.Create<GetUsersResponse>();

        _mediator.Send(ExtendedArg.Is(request), _cancellationToken).Returns(response);

        //Act
        var actualResult = await _sut.GetUsers(request.PageNumber, request.PageSize, _cancellationToken);

        //Assert
        actualResult.Should().BeOfType<OkObjectResult>();
        actualResult.As<OkObjectResult>().Value.Should().BeEquivalentTo(response);
    }

    [Test]
    public async Task ReverseStatus_ValidId_ReturnsNoContent()
    {
        //Arrange
        var request = _fixture.Create<ReverseStatusCommand>();

        //Act
        var actualResult = await _sut.ReverseStatus(request.UserId, _cancellationToken);

        //Assert
        actualResult.Should().BeOfType<NoContentResult>();

        await _mediator.Received(1).Send(ExtendedArg.Is(request), _cancellationToken);
    }

    [Test]
    public async Task DeleteUser_ValidId_ReturnsNoContent()
    {
        //Arrange
        var request = _fixture.Create<DeleteUserCommand>();

        //Act
        var actualResult = await _sut.DeleteUser(request.Email, _cancellationToken);

        //Assert
        actualResult.Should().BeOfType<NoContentResult>();

        await _mediator.Received(1).Send(ExtendedArg.Is(request), _cancellationToken);
    }

    [Test]
    public async Task GetTodos_ValidData_ReturnsTodos()
    {
        //Arrange
        var request = _fixture.Create<GetTodosByAdminQuery>();
        var response = _fixture.Create<GetTodosByAdminResponse>();

        _mediator.Send(ExtendedArg.Is(request), _cancellationToken).Returns(response);

        //Act
        var actualResult = await _sut.GetTodos(request.PageNumber, request.PageSize, _cancellationToken);

        //Assert
        actualResult.Should().BeOfType<OkObjectResult>();

        ((OkObjectResult)actualResult).Value.Should().Be(response);
    }

    [Test]
    public async Task DeleteTodo_ValidData_ReturnsNoContentResult()
    {
        //Arrange
        const int id = 10;

        //Act
        var actualResult = await _sut.DeleteTodo(id, _cancellationToken);

        //Assert
        actualResult.Should().BeOfType<NoContentResult>();
    }
}