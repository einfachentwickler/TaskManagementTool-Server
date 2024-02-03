using AutoFixture;
using AutoFixture.AutoNSubstitute;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using System.Net;
using TaskManagementTool.BusinessLogic.Dto.AuthModels;
using TaskManagementTool.BusinessLogic.Interfaces;
using TaskManagementTool.BusinessLogic.ViewModels;
using TaskManagementTool.BusinessLogic.ViewModels.AuthModels;
using TaskManagementTool.Host.Controllers;

namespace Host.UnitTests.Controllers;

[TestFixture]
public class AuthControllerTests
{
    private IFixture fixture;
    private IAuthHandler authHandler;

    private AuthController sut;

    [SetUp]
    public void Setup()
    {
        fixture = new Fixture().Customize(new AutoNSubstituteCustomization());

        authHandler = fixture.Freeze<IAuthHandler>();

        sut = new AuthController(authHandler);
    }

    [Test]
    public async Task Register_ValidData_Returns200()
    {
        //Arrange
        var registerDto = fixture.Create<RegisterDto>();

        var response = fixture.Build<UserManagerResponse>().With(x => x.IsSuccess, true).Create();

        authHandler.RegisterUserAsync(registerDto).Returns(response);

        //Act
        IActionResult actualResult = await sut.Register(registerDto);

        //Assert
        actualResult.Should().BeOfType<OkObjectResult>();

        actualResult.As<OkObjectResult>().StatusCode.Should().Be((int)HttpStatusCode.OK);
        actualResult.As<OkObjectResult>().Value.Should().BeEquivalentTo(response);
    }

    [Test]
    public async Task Register_InvalidData_Returns400()
    {
        //Arrange
        var registerDto = fixture.Create<RegisterDto>();

        var response = fixture.Build<UserManagerResponse>().With(x => x.IsSuccess, false).Create();

        authHandler.RegisterUserAsync(registerDto).Returns(response);

        //Act
        IActionResult actualResult = await sut.Register(registerDto);

        //Assert
        actualResult.Should().BeOfType<BadRequestObjectResult>();

        actualResult.As<BadRequestObjectResult>().StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        actualResult.As<BadRequestObjectResult>().Value.Should().BeEquivalentTo(response);
    }

    [Test]
    public async Task Login_ValidData_Returns200()
    {
        //Arrange
        var loginDto = fixture.Create<LoginDto>();

        var response = fixture.Build<UserManagerResponse>().With(x => x.IsSuccess, true).Create();

        authHandler.LoginUserAsync(loginDto).Returns(response);

        //Act
        IActionResult actualResult = await sut.Login(loginDto);

        //Assert
        actualResult.Should().BeOfType<OkObjectResult>();

        actualResult.As<OkObjectResult>().StatusCode.Should().Be((int)HttpStatusCode.OK);
        actualResult.As<OkObjectResult>().Value.Should().BeEquivalentTo(response);
    }

    [Test]
    public async Task Login_InvalidData_Returns400()
    {
        //Arrange
        var loginDto = fixture.Create<LoginDto>();

        var response = fixture.Build<UserManagerResponse>().With(x => x.IsSuccess, false).Create();

        authHandler.LoginUserAsync(loginDto).Returns(response);

        //Act
        IActionResult actualResult = await sut.Login(loginDto);

        //Assert
        actualResult.Should().BeOfType<UnauthorizedObjectResult>();

        actualResult.As<UnauthorizedObjectResult>().StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        actualResult.As<UnauthorizedObjectResult>().Value.Should().BeEquivalentTo(response);
    }

    [Test]
    public async Task ResetPasswordAsync_ValidData_Returns200()
    {
        //Arrange
        var request = fixture.Create<ResetPasswordDto>();

        var expectedResult = fixture
            .Build<UserManagerResponse>()
            .With(request => request.IsSuccess, true)
            .Create();

        authHandler.ResetPasswordAsync(request).Returns(expectedResult);

        //Act
        IActionResult actualResult = await sut.ResetPassword(request);

        //Assert
        actualResult.Should().BeOfType<OkObjectResult>();

        ((OkObjectResult)actualResult).Value.Should().BeEquivalentTo(expectedResult);

        await authHandler.Received(1).ResetPasswordAsync(request);
    }

    [Test]
    public async Task ResetPasswordAsync_ValidData_Returns400()
    {
        //Arrange
        var request = fixture.Create<ResetPasswordDto>();

        var expectedResult = fixture
            .Build<UserManagerResponse>()
            .With(request => request.IsSuccess, false)
            .Create();

        authHandler.ResetPasswordAsync(request).Returns(expectedResult);

        //Act
        IActionResult actualResult = await sut.ResetPassword(request);

        //Assert
        actualResult.Should().BeOfType<BadRequestObjectResult>();

        ((BadRequestObjectResult)actualResult).Value.Should().BeEquivalentTo(expectedResult);

        await authHandler.Received(1).ResetPasswordAsync(request);
    }
}