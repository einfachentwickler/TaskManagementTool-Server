using Application.Commands.Auth.Login;
using Application.Commands.Auth.Login.Models;
using Application.Commands.Utils.Jwt;
using Application.Commands.Wrappers;
using Application.Constants;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Infrastructure.Entities;
using NSubstitute;
using NUnit.Framework;
using System.IdentityModel.Tokens.Jwt;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace BusinessLogic.UnitTests.Commands.Auth.Login;

[TestFixture]
public class UserLoginHandlerTests
{
    private IUserManagerWrapper userManager;
    private IValidator<UserLoginRequest> requestValidator;
    private IJwtSecurityTokenBuilder jwtSecurityTokenBuilder;

    private IFixture fixture;

    private UserLoginHandler sut;

    [SetUp]
    public void Setup()
    {
        fixture = new Fixture().Customize(new AutoNSubstituteCustomization());

        userManager = fixture.Freeze<IUserManagerWrapper>();
        requestValidator = fixture.Freeze<IValidator<UserLoginRequest>>();
        jwtSecurityTokenBuilder = fixture.Freeze<IJwtSecurityTokenBuilder>();

        sut = fixture.Create<UserLoginHandler>();
    }

    [Test]
    public async Task Handle_ValidRequest_ReturnsToken()
    {
        //Arrange
        const string expectedTokenAsString = "token";
        UserLoginRequest request = new();
        var token = fixture.Create<CancellationToken>();
        var identityUser = fixture.Build<UserEntity>().With(user => user.IsBlocked, false).Create();
        var expectedToken = new JwtSecurityToken();

        requestValidator.ValidateAsync(request, token).Returns(new ValidationResult { Errors = [] });
        userManager.FindByEmailAsync(request.Email).Returns(identityUser);
        userManager.CheckPasswordAsync(identityUser, request.Password).Returns(true);
        jwtSecurityTokenBuilder.Build(identityUser, request).Returns((expectedTokenAsString, expectedToken));

        //Act
        UserLoginResponse actualResult = await sut.Handle(request, token);

        //Assert
        actualResult.Should().BeEquivalentTo(new UserLoginResponse
        {
            Message = expectedTokenAsString,
            IsSuccess = true,
            ExpirationDate = expectedToken.ValidTo
        });
    }

    [Test]
    public async Task Handle_InvalidRequest_ReturnsValidationErrors()
    {
        //Arrange
        UserLoginRequest request = new();
        var token = fixture.Create<CancellationToken>();
        var validationFailures = fixture.CreateMany<ValidationFailure>(2).ToList();
        ValidationResult validationResult = new() { Errors = validationFailures };

        requestValidator.ValidateAsync(request, token).Returns(validationResult);

        //Act
        UserLoginResponse actualResult = await sut.Handle(request, token);

        //Assert
        actualResult.Should().BeEquivalentTo(new UserLoginResponse
        {
            Message = UserManagerResponseMessages.USER_WAS_NOT_CREATED,
            IsSuccess = false,
            Errors = validationResult.Errors.ConvertAll(identityError => identityError.ErrorCode)
        });

        await userManager.DidNotReceiveWithAnyArgs().FindByEmailAsync(Arg.Any<string>());
        await userManager.DidNotReceiveWithAnyArgs().CheckPasswordAsync(Arg.Any<UserEntity>(), Arg.Any<string>());
        jwtSecurityTokenBuilder.DidNotReceiveWithAnyArgs().Build(Arg.Any<UserEntity>(), Arg.Any<UserLoginRequest>());
    }

    [Test]
    public async Task Handle_UserNotFound_ReturnsError()
    {
        //Arrange
        UserLoginRequest request = new();
        var token = fixture.Create<CancellationToken>();
        UserEntity? identityUser = null;

        requestValidator.ValidateAsync(request, token).Returns(new ValidationResult { Errors = [] });
        userManager.FindByEmailAsync(request.Email).Returns(identityUser);

        //Act
        UserLoginResponse actualResult = await sut.Handle(request, token);

        //Assert
        actualResult.Should().BeEquivalentTo(new UserLoginResponse
        {
            Message = UserManagerResponseMessages.USER_DOES_NOT_EXIST,
            IsSuccess = false
        });

        await userManager.DidNotReceiveWithAnyArgs().CheckPasswordAsync(Arg.Any<UserEntity>(), Arg.Any<string>());
        jwtSecurityTokenBuilder.DidNotReceiveWithAnyArgs().Build(Arg.Any<UserEntity>(), Arg.Any<UserLoginRequest>());
    }

    [Test]
    public async Task Handle_UserIsBlocked_ReturnsError()
    {
        //Arrange
        UserLoginRequest request = new();
        var token = fixture.Create<CancellationToken>();
        var identityUser = fixture.Build<UserEntity>().With(user => user.IsBlocked, true).Create();

        requestValidator.ValidateAsync(request, token).Returns(new ValidationResult { Errors = [] });
        userManager.FindByEmailAsync(request.Email).Returns(identityUser);

        //Act
        UserLoginResponse actualResult = await sut.Handle(request, token);

        //Assert
        actualResult.Should().BeEquivalentTo(new UserLoginResponse
        {
            Message = UserManagerResponseMessages.BLOCKED_EMAIL,
            IsSuccess = false
        });

        await userManager.DidNotReceiveWithAnyArgs().CheckPasswordAsync(Arg.Any<UserEntity>(), Arg.Any<string>());
        jwtSecurityTokenBuilder.DidNotReceiveWithAnyArgs().Build(Arg.Any<UserEntity>(), Arg.Any<UserLoginRequest>());
    }

    [Test]
    public async Task Handle_InalidCredentials_ReturnsError()
    {
        //Arrange
        UserLoginRequest request = new();
        var token = fixture.Create<CancellationToken>();
        var identityUser = fixture.Build<UserEntity>().With(user => user.IsBlocked, false).Create();

        requestValidator.ValidateAsync(request, token).Returns(new ValidationResult { Errors = [] });
        userManager.FindByEmailAsync(request.Email).Returns(identityUser);
        userManager.CheckPasswordAsync(identityUser, request.Password).Returns(false);

        //Act
        UserLoginResponse actualResult = await sut.Handle(request, token);

        //Assert
        actualResult.Should().BeEquivalentTo(new UserLoginResponse
        {
            Message = UserManagerResponseMessages.INVALID_CREDENTIALS,
            IsSuccess = false
        });

        jwtSecurityTokenBuilder.DidNotReceiveWithAnyArgs().Build(Arg.Any<UserEntity>(), Arg.Any<UserLoginRequest>());
    }
}