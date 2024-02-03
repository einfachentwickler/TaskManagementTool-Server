﻿using AutoFixture;
using AutoFixture.AutoNSubstitute;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
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

    private AdminController sut;

    [SetUp]
    public void Setup()
    {
        fixture = new Fixture().Customize(new AutoNSubstituteCustomization());

        adminHandler = fixture.Freeze<IAdminHandler>();
        todoHandler = fixture.Freeze<ITodoHandler>();

        sut = new AdminController(adminHandler, todoHandler);
    }

    [Test]
    public async Task GetUsers_Paging_ReturnsOk()
    {
        //Arrange
        int pageNumber = fixture.Create<int>();
        int pageSize = fixture.Create<int>();

        var users = fixture.CreateMany<UserDto>();

        adminHandler.GetAsync(pageNumber, pageSize).Returns(users);

        //Act
        IActionResult response = await sut.GetUsers(pageNumber, pageSize);

        //Assert
        response.Should().BeOfType<OkObjectResult>();
        response.As<OkObjectResult>().Value.Should().BeEquivalentTo(users);
    }

    [Test]
    public async Task ReverseStatus_ValidId_ReturnsNoContent()
    {
        //Arrange
        var userId = fixture.Create<string>();

        //Act
        IActionResult response = await sut.ReverseStatus(userId);

        //Assert
        await adminHandler.Received(1).BlockOrUnblockAsync(userId);

        response.Should().BeOfType<NoContentResult>();
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