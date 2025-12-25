using AutoFixture;
using AutoFixture.AutoNSubstitute;
using BusinessLogic.UnitTests.Utils;
using FluentAssertions;
using Infrastructure.Contracts;
using Infrastructure.Data.Entities;
using MediatR;
using NSubstitute;
using NUnit.Framework;
using System.Linq.Expressions;
using TaskManagementTool.BusinessLogic.Commands.Admin.DeleteTodoByAdmin;
using TaskManagementTool.BusinessLogic.Commands.Admin.DeleteTodoByAdmin.Models;

namespace BusinessLogic.UnitTests.Commands.Admin;

[TestFixture]
public class DeleteTodoByAdminHandlerTests
{
    private IFixture fixture;

    private ITodoRepository todoRepository;

    private DeleteTodoByAdminHandler sut;

    [SetUp]
    public void Setup()
    {
        fixture = new Fixture().Customize(new AutoNSubstituteCustomization());

        todoRepository = fixture.Freeze<ITodoRepository>();

        sut = fixture.Create<DeleteTodoByAdminHandler>();
    }

    [Test]
    public async Task Handle_ValidData_Received()
    {
        //Arrange
        var request = fixture.Create<DeleteTodoByAdminRequest>();
        var token = fixture.Create<CancellationToken>();

        Expression<Func<ToDoEntity, bool>> predicate = entry => entry.Id == request.TodoId;

        //Act
        Unit actualResult = await sut.Handle(request, token);

        //Assert
        actualResult.Should().BeEquivalentTo(new Unit());

        await todoRepository.Received(1).DeleteAsync(ExtendedArg.Is(predicate));
    }
}