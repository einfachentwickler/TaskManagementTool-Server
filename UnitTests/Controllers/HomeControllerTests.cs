﻿using AutoFixture;
using AutoFixture.AutoNSubstitute;
using FluentAssertions;
using Host.UnitTests.Utils;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using TaskManagementTool.BusinessLogic.Commands.Home.CreateTodo.Models;
using TaskManagementTool.BusinessLogic.Commands.Home.DeleteTodo.Models;
using TaskManagementTool.BusinessLogic.Commands.Home.GetTodoById.Models;
using TaskManagementTool.BusinessLogic.Commands.Home.GetTodos.Models;
using TaskManagementTool.BusinessLogic.Commands.Home.UpdateTodo.Models;
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
            .Build<GetTodosRequest>()
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
          .Build<GetTodoByIdRequest>()
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
            .Build<CreateTodoRequest>()
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
          .Build<UpdateTodoRequest>()
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

        DeleteTodoRequest request = new() { HttpContext = httpContextAccessor.HttpContext, TodoId = id };

        DeleteTodoResponse response = new() { IsSuccess = true };

        mediator.Send(ExtendedArg.Is(request)).Returns(response);

        //Act
        IActionResult actualResult = await sut.Delete(id);

        //Assert
        actualResult.Should().BeOfType<OkObjectResult>();

        ((OkObjectResult)actualResult).Value.Should().Be(response);
    }
}