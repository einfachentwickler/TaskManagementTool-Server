namespace Application.UnitTests.Commands.Admin;

//todo fix tests
//[TestFixture]
//public class DeleteTodoByAdminHandlerTests
//{
//    private IFixture fixture;

//    private ITodoRepository todoRepository;

//    private DeleteTodoByAdminHandler sut;

//    [SetUp]
//    public void Setup()
//    {
//        fixture = new Fixture().Customize(new AutoNSubstituteCustomization());

//        todoRepository = fixture.Freeze<ITodoRepository>();

//        sut = fixture.Create<DeleteTodoByAdminHandler>();
//    }

//    [Test]
//    public async Task Handle_ValidData_Received()
//    {
//        //Arrange
//        var request = fixture.Create<DeleteTodoByAdminRequest>();
//        var token = fixture.Create<CancellationToken>();

//        Expression<Func<ToDoEntity, bool>> predicate = entry => entry.Id == request.TodoId;

//        //Act
//        Unit actualResult = await sut.Handle(request, token);

//        //Assert
//        actualResult.Should().BeEquivalentTo(new Unit());

//        await todoRepository.Received(1).DeleteAsync(ExtendedArg.Is(predicate));
//    }
//}