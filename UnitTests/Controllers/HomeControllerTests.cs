using Application.Commands.Home.CreateTodo.Models;
using Application.Commands.Home.DeleteTodo.Models;
using Application.Commands.Home.UpdateTodo.Models;
using Application.Queries.Home.GetTodoById.Models;
using Application.Queries.Home.GetTodos.Models;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using FluentAssertions;
using Host.UnitTests.Utils;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using TaskManagementTool.Host.Controllers;

namespace Host.UnitTests.Controllers;

[TestFixture]
public class HomeControllerTests
{
    private IFixture fixture;

    private IMediator mediator;
    private IHttpContextAccessor httpContextAccessor;

    private HomeController sut;

    [SetUp]
    public void Setup()
    {
        fixture = new Fixture().Customize(new AutoNSubstituteCustomization());

        httpContextAccessor = fixture.Freeze<IHttpContextAccessor>();
        mediator = fixture.Freeze<IMediator>();

        sut = new HomeController(mediator, httpContextAccessor);
    }

    [Test]
    public async Task GetUsersTodos_ValidData_ReturnsTodos()
    {
        //Arrange
        var request = fixture
            .Build<GetTodosQuery>()
            .With(request => request.HttpContext, httpContextAccessor.HttpContext)
            .Create();

        GetTodosResponse response = fixture.Create<GetTodosResponse>();

        mediator.Send(ExtendedArg.Is(request)).Returns(response);

        //Act
        IActionResult actualResult = await sut.GetTodos(request.PageNumber, request.PageSize);

        //Assert
        actualResult.Should().BeOfType<OkObjectResult>();

        ((OkObjectResult)actualResult).Value.Should().BeEquivalentTo(response);
    }

    [Test]
    public async Task GetById_ValidData_ReturnsTodo()
    {
        //Arrange
        var request = fixture
          .Build<GetTodoByIdQuery>()
          .With(request => request.HttpContext, httpContextAccessor.HttpContext)
          .Create();

        var response = fixture.Create<GetTodoByIdResponse>();

        mediator.Send(ExtendedArg.Is(request)).Returns(response);

        //Act
        IActionResult actualResult = await sut.GetById(request.TodoId);

        //Assert
        actualResult.Should().BeOfType<OkObjectResult>();

        ((OkObjectResult)actualResult).Value.Should().Be(response);
    }

    [Test]
    public async Task Create_ValidData_ReturnsCreatedTodo()
    {
        //Arrange
        var request = fixture
            .Build<CreateTodoCommand>()
            .With(request => request.HttpContext, httpContextAccessor.HttpContext)
            .Create();

        var response = fixture.Create<CreateTodoResponse>();

        mediator.Send(ExtendedArg.Is(request)).Returns(response);

        //Act
        IActionResult actualResult = await sut.Create(request.CreateTodoDto);

        //Assert
        actualResult.Should().BeOfType<OkObjectResult>();

        ((OkObjectResult)actualResult).Value.Should().BeEquivalentTo(response);
    }

    [Test]
    public async Task Update_ValidData_ReturnsUpdatedTodo()
    {
        var request = fixture
          .Build<UpdateTodoCommand>()
          .With(request => request.HttpContext, httpContextAccessor.HttpContext)
          .Create();

        var response = fixture.Create<UpdateTodoResponse>();

        mediator.Send(ExtendedArg.Is(request)).Returns(response);

        //Act
        IActionResult actualResult = await sut.Update(request.UpdateTodoDto);

        //Assert
        actualResult.Should().BeOfType<OkObjectResult>();

        ((OkObjectResult)actualResult).Value.Should().Be(response);
    }

    [Test]
    public async Task Delete_ValidData_ReturnsOk()
    {
        //Arrange
        const int id = 2;

        DeleteTodoCommand request = new() { HttpContext = httpContextAccessor.HttpContext, TodoId = id };

        DeleteTodoResponse response = new() { IsSuccess = true };

        mediator.Send(ExtendedArg.Is(request)).Returns(response);

        //Act
        IActionResult actualResult = await sut.Delete(id);

        //Assert
        actualResult.Should().BeOfType<OkObjectResult>();

        ((OkObjectResult)actualResult).Value.Should().Be(response);
    }
}