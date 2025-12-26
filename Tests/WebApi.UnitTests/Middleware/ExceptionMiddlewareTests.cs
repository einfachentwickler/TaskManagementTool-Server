using AutoFixture;
using AutoFixture.AutoNSubstitute;
using LoggerService;
using NUnit.Framework;

namespace WebApi.UnitTests.Middleware;

[TestFixture]
public class ExceptionMiddlewareTests
{
    private IFixture fixture;
    private ILoggerManager loggerManager;

    [SetUp]
    public void Setup()
    {
        fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        loggerManager = fixture.Freeze<ILoggerManager>();
    }
}