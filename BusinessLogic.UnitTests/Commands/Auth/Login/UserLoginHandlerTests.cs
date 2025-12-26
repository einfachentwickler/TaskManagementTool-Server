using Application.Commands.Auth.Login;
using Application.Commands.Auth.Login.Models;
using Application.Commands.Utils.Jwt;
using Application.Commands.Wrappers;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using FluentValidation;
using NUnit.Framework;

namespace BusinessLogic.UnitTests.Commands.Auth.Login;

[TestFixture]
public class UserLoginHandlerTests
{
    private IUserManagerWrapper userManager;
    private IValidator<UserLoginCommand> requestValidator;
    private IJwtSecurityTokenBuilder jwtSecurityTokenBuilder;

    private IFixture fixture;

    private UserLoginHandler sut;

    [SetUp]
    public void Setup()
    {
        fixture = new Fixture().Customize(new AutoNSubstituteCustomization());

        userManager = fixture.Freeze<IUserManagerWrapper>();
        requestValidator = fixture.Freeze<IValidator<UserLoginCommand>>();
        jwtSecurityTokenBuilder = fixture.Freeze<IJwtSecurityTokenBuilder>();

        sut = fixture.Create<UserLoginHandler>();
    }
}