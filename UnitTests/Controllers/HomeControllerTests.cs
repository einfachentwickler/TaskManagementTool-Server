using AutoFixture;
using AutoFixture.AutoNSubstitute;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using TaskManagementTool.BusinessLogic.Commands.Utils;
using TaskManagementTool.BusinessLogic.Interfaces;
using TaskManagementTool.BusinessLogic.ViewModels;
using TaskManagementTool.BusinessLogic.ViewModels.ToDoModels;
using TaskManagementTool.Host.Controllers;

namespace Host.UnitTests.Controllers;

[TestFixture]
public class HomeControllerTests
{
    private IFixture fixture;

    private ITodoHandler todoHandler;
    private IHttpContextAccessor httpContextAccessor;
    private IAuthUtils authUtils;

    private HomeController sut;

    [SetUp]
    public void Setup()
    {
        fixture = new Fixture().Customize(new AutoNSubstituteCustomization());

        todoHandler = fixture.Freeze<ITodoHandler>();
        httpContextAccessor = fixture.Freeze<IHttpContextAccessor>();
        authUtils = fixture.Freeze<IAuthUtils>();

        sut = new HomeController(todoHandler, httpContextAccessor, authUtils);
    }

    [Test]
    public async Task GetUsersTodos_ValidData_ReturnsTodos()
    {
        //Arrange
        const string userId = "user id";
        const int pageNumber = 5;
        const int pageSize = 10;

        IEnumerable<TodoDto> todos = fixture.CreateMany<TodoDto>(5);

        authUtils.GetUserId(httpContextAccessor.HttpContext).Returns(userId);

        todoHandler.GetAsync(userId, pageNumber, pageSize).Returns(todos);

        //Act
        IActionResult actualResult = await sut.GetUsersTodos(pageNumber, pageSize);

        //Assert
        actualResult.Should().BeOfType<OkObjectResult>();

        ((OkObjectResult)actualResult).Value.Should().Be(todos);
    }

    [Test]
    public async Task GetById_ValidData_ReturnsTodo()
    {
        //Arrange
        const int id = 5;

        var todo = fixture.Create<TodoDto>();

        todoHandler.FindByIdAsync(id).Returns(todo);

        authUtils.IsAllowedAction(httpContextAccessor.HttpContext, id).Returns(true);

        //Act
        IActionResult actualResult = await sut.GetById(id);

        //Assert
        actualResult.Should().BeOfType<OkObjectResult>();

        ((OkObjectResult)actualResult).Value.Should().Be(todo);
    }

    [Test]
    public async Task GetById_NotAllowedAction_ReturnsForbid()
    {
        //Arrange
        const int id = 5;

        var todo = fixture.Create<TodoDto>();

        todoHandler.FindByIdAsync(id).Returns(todo);

        authUtils.IsAllowedAction(httpContextAccessor.HttpContext, id).Returns(false);

        //Act
        IActionResult actualResult = await sut.GetById(id);

        //Assert
        actualResult.Should().BeOfType<ForbidResult>();
    }

    [Test]
    public async Task Create_ValidData_ReturnsCreatedTodo()
    {
        //Arrange
        const string userId = "user id";

        var createTodoDto = fixture.Create<CreateTodoDto>();
        var expectedResult = fixture.Create<TodoDto>();

        authUtils.GetUserId(httpContextAccessor.HttpContext).Returns(userId);

        todoHandler.CreateAsync(createTodoDto).Returns(expectedResult);

        //Act
        IActionResult actualResult = await sut.Create(createTodoDto);

        //Assert
        actualResult.Should().BeOfType<OkObjectResult>();

        ((OkObjectResult)actualResult).Value.Should().Be(expectedResult);

        authUtils.Received(1).GetUserId(httpContextAccessor.HttpContext);
    }

    [Test]
    public async Task Update_ValidData_ReturnsUpdatedTodo()
    {
        //Arrange
        const string userId = "user id";

        var updateTodoDto = fixture.Create<UpdateTodoDto>();
        var expectedResult = fixture.Create<TodoDto>();

        authUtils.IsAllowedAction(httpContextAccessor.HttpContext, updateTodoDto.Id).Returns(true);

        authUtils.GetUserId(httpContextAccessor.HttpContext).Returns(userId);

        todoHandler.UpdateAsync(updateTodoDto).Returns(expectedResult);

        //Act
        IActionResult actualResult = await sut.Update(updateTodoDto);

        //Assert
        actualResult.Should().BeOfType<OkObjectResult>();

        ((OkObjectResult)actualResult).Value.Should().Be(expectedResult);
    }

    [Test]
    public async Task Update_NotAllowedAction_ReturnsForbid()
    {
        //Arrange
        const string userId = "user id";

        var updateTodoDto = fixture.Create<UpdateTodoDto>();

        authUtils.IsAllowedAction(httpContextAccessor.HttpContext, updateTodoDto.Id).Returns(false);

        authUtils.GetUserId(httpContextAccessor.HttpContext).Returns(userId);

        //Act
        IActionResult actualResult = await sut.Update(updateTodoDto);

        //Assert
        actualResult.Should().BeOfType<ForbidResult>();

        await todoHandler.DidNotReceiveWithAnyArgs().UpdateAsync(Arg.Any<UpdateTodoDto>());
    }

    [Test]
    public async Task Delete_ValidData_ReturnsNoContent()
    {
        //Arrange
        const int id = 2;

        authUtils.IsAllowedAction(httpContextAccessor.HttpContext, id).Returns(true);

        //Act
        IActionResult actualResult = await sut.Delete(id);

        //Assert
        actualResult.Should().BeOfType<NoContentResult>();

        await todoHandler.Received(1).DeleteAsync(Arg.Any<int>());
    }

    [Test]
    public async Task Delete_NotAllowedAction_ReturnsForbid()
    {
        //Arrange
        const int id = 2;

        authUtils.IsAllowedAction(httpContextAccessor.HttpContext, id).Returns(false);

        //Act
        IActionResult actualResult = await sut.Delete(id);

        //Assert
        actualResult.Should().BeOfType<ForbidResult>();

        await todoHandler.DidNotReceiveWithAnyArgs().DeleteAsync(Arg.Any<int>());
    }
}