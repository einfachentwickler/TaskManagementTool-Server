using Application.Commands.Auth.Login.Models;
using Application.Commands.Auth.Register.Models;
using Application.Commands.Auth.ResetPassword.Models;
using Application.Constants;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using System.Net;
using TaskManagementTool.Host.Controllers;

namespace Host.UnitTests.Controllers;

[TestFixture]
public class AuthControllerTests
{
    private IFixture fixture;
    private IMediator mediator;

    private AuthController sut;

    [SetUp]
    public void Setup()
    {
        fixture = new Fixture().Customize(new AutoNSubstituteCustomization());

        mediator = fixture.Freeze<IMediator>();

        sut = new AuthController(mediator);
    }

    [Test]
    public async Task Register_ValidData_Returns200()
    {
        //Arrange
        var registerDto = fixture.Create<UserRegisterRequest>();

        UserRegisterResponse expectedResult = new()
        {
            IsSuccess = true
        };

        mediator.Send(registerDto).Returns(expectedResult);

        //Act
        IActionResult actualResult = await sut.Register(registerDto);

        //Assert
        actualResult.Should().BeOfType<OkObjectResult>();

        actualResult.As<OkObjectResult>().StatusCode.Should().Be((int)HttpStatusCode.OK);
        actualResult.As<OkObjectResult>().Value.Should().BeEquivalentTo(expectedResult);
    }

    [Test]
    public async Task Register_InvalidData_Returns400()
    {
        //Arrange
        var registerDto = fixture.Create<UserRegisterRequest>();

        UserRegisterResponse expectedResult = new()
        {
            IsSuccess = false,
            Message = UserManagerResponseMessages.INVALID_CREDENTIALS,
        };

        mediator.Send(registerDto).Returns(expectedResult);

        //Act
        IActionResult actualResult = await sut.Register(registerDto);

        //Assert
        actualResult.Should().BeOfType<BadRequestObjectResult>();

        actualResult.As<BadRequestObjectResult>().StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        actualResult.As<BadRequestObjectResult>().Value.Should().BeEquivalentTo(expectedResult);
    }

    [Test]
    public async Task Login_ValidData_Returns200()
    {
        //Arrange
        var loginDto = fixture.Create<UserLoginRequest>();

        UserLoginResponse expectedResult = new() { IsSuccess = true, Token = "token" };

        mediator.Send(loginDto).Returns(expectedResult);

        //Act
        IActionResult actualResult = await sut.Login(loginDto);

        //Assert
        actualResult.Should().BeOfType<OkObjectResult>();

        actualResult.As<OkObjectResult>().StatusCode.Should().Be((int)HttpStatusCode.OK);
        actualResult.As<OkObjectResult>().Value.Should().BeEquivalentTo(expectedResult);
    }

    [Test]
    public async Task Login_InvalidData_Returns400()
    {
        //Arrange
        var loginDto = fixture.Create<UserLoginRequest>();

        UserLoginResponse expectedResult = new() { IsSuccess = false, Message = UserManagerResponseMessages.INVALID_CREDENTIALS };

        mediator.Send(loginDto).Returns(expectedResult);

        //Act
        IActionResult actualResult = await sut.Login(loginDto);

        //Assert
        actualResult.Should().BeOfType<UnauthorizedObjectResult>();

        actualResult.As<UnauthorizedObjectResult>().StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        actualResult.As<UnauthorizedObjectResult>().Value.Should().BeEquivalentTo(expectedResult);
    }

    [Test]
    public async Task ResetPasswordAsync_ValidData_Returns200()
    {
        //Arrange
        var request = fixture.Create<ResetPasswordRequest>();

        ResetPasswordResponse expectedResult = new() { IsSuccess = true };

        mediator.Send(request).Returns(expectedResult);

        //Act
        IActionResult actualResult = await sut.ResetPassword(request);

        //Assert
        actualResult.Should().BeOfType<OkObjectResult>();

        ((OkObjectResult)actualResult).Value.Should().BeEquivalentTo(expectedResult);

        await mediator.Received(1).Send(request);
    }

    [Test]
    public async Task ResetPasswordAsync_ValidData_Returns400()
    {
        //Arrange
        var request = fixture.Create<ResetPasswordRequest>();

        ResetPasswordResponse expectedResult = new() { IsSuccess = false, Message = UserManagerResponseMessages.INVALID_CREDENTIALS };

        mediator.Send(request).Returns(expectedResult);

        //Act
        IActionResult actualResult = await sut.ResetPassword(request);

        //Assert
        actualResult.Should().BeOfType<BadRequestObjectResult>();

        ((BadRequestObjectResult)actualResult).Value.Should().BeEquivalentTo(expectedResult);

        await mediator.Received(1).Send(request);
    }
}