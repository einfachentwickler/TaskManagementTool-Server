using AutoFixture;
using AutoFixture.AutoNSubstitute;
using MediatR;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using WebApi.Controllers;

namespace WebApi.UnitTests.Controllers;

[TestFixture]
public class HomeControllerTests
{
    private IFixture fixture;

    private IMediator mediator;
    private IHttpContextAccessor httpContextAccessor;

    private HomeController sut;

    [SetUp]
    public void Setup()
    {
        fixture = new Fixture().Customize(new AutoNSubstituteCustomization());

        httpContextAccessor = fixture.Freeze<IHttpContextAccessor>();
        mediator = fixture.Freeze<IMediator>();

        sut = new HomeController(mediator, httpContextAccessor);
    }
}