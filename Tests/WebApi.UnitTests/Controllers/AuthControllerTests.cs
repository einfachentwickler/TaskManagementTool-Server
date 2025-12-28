using Application.Commands.Auth.Login.Models;
using Application.Commands.Auth.RefreshToken.Models;
using Application.Commands.Auth.Register.Models;
using Application.Commands.Auth.ResetPassword.Models;
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
public class AuthControllerTests
{
    private IFixture _fixture;
    private IMediator _mediator;
    private CancellationToken _cancellationToken;

    private AuthController _sut;

    [SetUp]
    public void Setup()
    {
        _fixture = new Fixture().Customize(new AutoNSubstituteCustomization());

        _mediator = _fixture.Freeze<IMediator>();
        _cancellationToken = _fixture.Create<CancellationToken>();

        _sut = new AuthController(_mediator);
    }

    [Test]
    public async Task Register_ValidRequest_ReturnsNoContent()
    {
        // Arrange
        var request = _fixture.Create<UserRegisterCommand>();

        // Act
        var actualResult = await _sut.Register(request, _cancellationToken);

        // Assert
        actualResult.Should().BeOfType<NoContentResult>();

        await _mediator.Received(1).Send(ExtendedArg.Is(request), _cancellationToken);
    }

    [Test]
    public async Task Login_ValidRequest_ReturnsOkWithResponse()
    {
        // Arrange
        var request = _fixture.Create<UserLoginCommand>();
        var response = _fixture.Create<UserLoginResponse>();

        _mediator.Send(ExtendedArg.Is(request), _cancellationToken).Returns(response);

        // Act
        var actualResult = await _sut.Login(request, _cancellationToken);

        // Assert
        actualResult.Should().BeOfType<OkObjectResult>();
        actualResult.As<OkObjectResult>().Value.Should().BeEquivalentTo(response);
    }

    [Test]
    public async Task ResetPassword_ValidRequest_ReturnsNoContent()
    {
        // Arrange
        var request = _fixture.Create<ResetPasswordCommand>();

        // Act
        var actualResult = await _sut.ResetPassword(request, _cancellationToken);

        // Assert
        actualResult.Should().BeOfType<NoContentResult>();

        await _mediator.Received(1).Send(ExtendedArg.Is(request), _cancellationToken);
    }

    [Test]
    public async Task Refresh_ValidRequest_Returns()
    {
        //Arrange
        var command = _fixture.Create<RefreshTokenCommand>();
        var response = _fixture.Create<UserLoginResponse>();

        _mediator.Send(command, _cancellationToken).Returns(response);

        //Act
        var actualResult = await _sut.Refresh(command, _cancellationToken);

        //Assert
        actualResult.Should().BeOfType<OkObjectResult>();

        actualResult.As<OkObjectResult>().Value.Should().BeEquivalentTo(response);
    }
}