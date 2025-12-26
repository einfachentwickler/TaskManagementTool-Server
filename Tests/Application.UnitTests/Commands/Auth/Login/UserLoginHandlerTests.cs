using Application.Commands.Auth.Login;
using Application.Commands.Auth.Login.Models;
using Application.Services.IdentityUserManagement;
using Application.Services.Jwt;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using FluentValidation;
using NUnit.Framework;

namespace Application.UnitTests.Commands.Auth.Login;

[TestFixture]
public class UserLoginHandlerTests
{
    private IIdentityUserManagerWrapper userManager;
    private IValidator<UserLoginCommand> requestValidator;
    private IJwtSecurityTokenBuilder jwtSecurityTokenBuilder;

    private IFixture fixture;

    private UserLoginHandler sut;

    [SetUp]
    public void Setup()
    {
        fixture = new Fixture().Customize(new AutoNSubstituteCustomization());

        userManager = fixture.Freeze<IIdentityUserManagerWrapper>();
        requestValidator = fixture.Freeze<IValidator<UserLoginCommand>>();
        jwtSecurityTokenBuilder = fixture.Freeze<IJwtSecurityTokenBuilder>();

        sut = fixture.Create<UserLoginHandler>();
    }
}