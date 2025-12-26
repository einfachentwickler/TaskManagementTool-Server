using NUnit.Framework;

namespace WebApi.UnitTests.Controllers;

[TestFixture]
public class AdminControllerTests
{
    //private IFixture fixture;
    //private IMediator mediator;

    //private AdminController sut;

    //[SetUp]
    //public void Setup()
    //{
    //    fixture = new Fixture().Customize(new AutoNSubstituteCustomization());

    //    mediator = fixture.Freeze<IMediator>();

    //    sut = new AdminController(mediator);
    //}

    //[Test]
    //public async Task GetUsers_Paging_ReturnsOk()
    //{
    //    //Arrange
    //    var request = fixture.Create<GetUsersQuery>();
    //    var response = fixture.Create<GetUsersResponse>();

    //    mediator.Send(ExtendedArg.Is(request)).Returns(response);

    //    //Act
    //    IActionResult actualResponse = await sut.GetUsers(request.PageNumber, request.PageSize);

    //    //Assert
    //    actualResponse.Should().BeOfType<OkObjectResult>();
    //    actualResponse.As<OkObjectResult>().Value.Should().BeEquivalentTo(response);
    //}

    //[Test]
    //public async Task ReverseStatus_ValidId_ReturnsNoContent()
    //{
    //    //Arrange
    //    var request = fixture.Create<ReverseStatusCommand>();

    //    //Act
    //    IActionResult actualResult = await sut.ReverseStatus(request.UserId);

    //    //Assert
    //    actualResult.Should().BeOfType<NoContentResult>();

    //    await mediator.Received(1).Send(ExtendedArg.Is(request));
    //}

    //[Test]
    //public async Task DeleteUser_ValidId_ReturnsNoContent()
    //{
    //    //Arrange
    //    var request = fixture.Create<DeleteUserCommand>();

    //    //Act
    //    IActionResult response = await sut.DeleteUser(request.Email);

    //    //Assert
    //    response.Should().BeOfType<NoContentResult>();

    //    await mediator.Received(1).Send(ExtendedArg.Is(request));
    //}

    //[Test]
    //public async Task GetTodos_ValidData_ReturnsTodos()
    //{
    //    //Arrange
    //    var request = fixture.Create<GetTodosByAdminQuery>();
    //    var response = fixture.Create<GetTodosByAdminResponse>();

    //    mediator.Send(ExtendedArg.Is(request)).Returns(response);

    //    //Act
    //    var actualResult = await sut.GetTodos(request.PageNumber, request.PageSize);

    //    //Assert
    //    actualResult.Should().BeOfType<OkObjectResult>();

    //    ((OkObjectResult)actualResult).Value.Should().Be(response);
    //}

    //[Test]
    //public async Task DeleteTodo_ValidData_ReturnsNoContentResult()
    //{
    //    //Arrange
    //    const int id = 10;

    //    //Act
    //    var actualResult = await sut.DeleteTodo(id);

    //    //Assert
    //    actualResult.Should().BeOfType<NoContentResult>();
    //}
}