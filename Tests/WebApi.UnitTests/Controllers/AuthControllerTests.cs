using AutoFixture;
using AutoFixture.AutoNSubstitute;
using MediatR;
using NUnit.Framework;
using WebApi.Controllers;

namespace WebApi.UnitTests.Controllers;

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
}