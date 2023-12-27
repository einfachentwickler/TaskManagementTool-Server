using AutoFixture;
using AutoFixture.AutoNSubstitute;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using TaskManagementTool.BusinessLogic.Interfaces;
using TaskManagementTool.BusinessLogic.ViewModels;
using TaskManagementTool.Host.Controllers;

namespace UnitTests.Host.Controllers;

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
}
