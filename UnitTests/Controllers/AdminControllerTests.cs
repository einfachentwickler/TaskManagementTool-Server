using AutoFixture;
using AutoFixture.AutoNSubstitute;
using FluentAssertions;
using Host.UnitTests.Utils;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using TaskManagementTool.BusinessLogic.Commands.Admin.GetUsers.Models;
using TaskManagementTool.BusinessLogic.Commands.Admin.ReverseStatus.Models;
using TaskManagementTool.BusinessLogic.Interfaces;
using TaskManagementTool.BusinessLogic.ViewModels;
using TaskManagementTool.Host.Controllers;

namespace Host.UnitTests.Controllers;

[TestFixture]
public class AdminControllerTests
{
    private IFixture fixture;
    private IAdminHandler adminHandler;
    private ITodoHandler todoHandler;
    private IMediator mediator;

    private AdminController sut;

    [SetUp]
    public void Setup()
    {
        fixture = new Fixture().Customize(new AutoNSubstituteCustomization());

        adminHandler = fixture.Freeze<IAdminHandler>();
        todoHandler = fixture.Freeze<ITodoHandler>();
        mediator = fixture.Freeze<IMediator>();

        sut = new AdminController(adminHandler, todoHandler, mediator);
    }

    [Test]
    public async Task GetUsers_Paging_ReturnsOk()
    {
        //Arrange
        var request = fixture.Create<GetUsersRequest>();
        var response = fixture.Create<GetUsersResponse>();

        mediator.Send(ExtendedArg.Is(request)).Returns(response);

        //Act
        IActionResult actualResponse = await sut.GetUsers(request.PageNumber, request.PageSize);

        //Assert
        actualResponse.Should().BeOfType<OkObjectResult>();
        actualResponse.As<OkObjectResult>().Value.Should().BeEquivalentTo(response);
    }

    [Test]
    public async Task ReverseStatus_ValidId_ReturnsNoContent()
    {
        //Arrange
        var request = fixture.Create<ReverseStatusRequest>();

        //Act
        IActionResult actualResult = await sut.ReverseStatus(request.UserId);

        //Assert
        actualResult.Should().BeOfType<NoContentResult>();

        await mediator.Received(1).Send(ExtendedArg.Is(request));
    }

    [Test]
    public async Task DeleteUser_ValidId_ReturnsNoContent()
    {
        //Arrange
        var userId = fixture.Create<string>();

        //Act
        IActionResult response = await sut.DeleteUser(userId);

        //Assert
        await adminHandler.Received(1).DeleteAsync(userId);

        response.Should().BeOfType<NoContentResult>();
    }

    [Test]
    public async Task GetTodos_ValidData_ReturnsTodos()
    {
        //Arrange
        const int pageSize = 10;
        const int pageNumber = 2;

        var todos = fixture.CreateMany<TodoDto>(5);

        todoHandler.GetAsync(pageSize, pageNumber).Returns(todos);

        //Act
        var actualResult = await sut.GetTodos(pageNumber, pageSize);

        //Assert
        actualResult.Should().BeOfType<OkObjectResult>();

        ((OkObjectResult)actualResult).Value.Should().Be(todos);
    }

    [Test]
    public async Task DeleteTodo_ValidData_ReturnsNoContentResult()
    {
        //Arrange
        const int id = 10;

        //Act
        var actualResult = await sut.DeleteTodo(id);

        //Assert
        await todoHandler.Received(1).DeleteAsync(id);

        actualResult.Should().BeOfType<NoContentResult>();
    }
}
