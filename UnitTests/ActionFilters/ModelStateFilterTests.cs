using AutoFixture;
using AutoFixture.AutoNSubstitute;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using NUnit.Framework;
using TaskManagementTool.BusinessLogic.Interfaces;
using TaskManagementTool.Host.ActionFilters;
using TaskManagementTool.Host.Controllers;

namespace Host.UnitTests.ActionFilters;

[TestFixture]
public class ModelStateFilterTests
{
    private IFixture fixture;
    private ModelStateFilter sut;
    private ActionExecutingContext context;

    [SetUp]
    public void Setup()
    {
        fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        sut = new ModelStateFilter();

        DefaultHttpContext httpContext = new();

        ActionContext actionContext = new()
        {
            HttpContext = httpContext,
            RouteData = new RouteData(),
            ActionDescriptor = new ActionDescriptor(),
        };

        context = new(
            actionContext,
            new List<IFilterMetadata>(),
            new Dictionary<string, object?>(),
            new AdminController(fixture.Freeze<IAdminHandler>(), fixture.Freeze<ITodoHandler>())
            );
    }

    [Test]
    public async Task OnActionExecuting_ValidModel_ResponseIsNot400()
    {
        //Act
        await sut.OnActionExecutionAsync(context, () => Task.FromResult<ActionExecutedContext>(null!));

        //Assert
        context.HttpContext.Response.StatusCode.Should().NotBe(StatusCodes.Status400BadRequest);
    }

    [Test]
    public async Task OnActionExecuting_InvalidModel_ResponseIs400()
    {
        //Arrange
        context.ModelState.AddModelError("error key", "error message");

        //Act
        await sut.OnActionExecutionAsync(context, () => Task.FromResult<ActionExecutedContext>(null!));

        //Assert
        context.HttpContext.Response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }
}