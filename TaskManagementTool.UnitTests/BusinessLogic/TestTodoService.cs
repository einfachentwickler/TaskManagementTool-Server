using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoMapper;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementTool.BusinessLogic.Contracts;
using TaskManagementTool.BusinessLogic.Services;
using TaskManagementTool.BusinessLogic.ViewModels;
using TaskManagementTool.DataAccess.Contracts;
using TaskManagementTool.Host.Profiles;

namespace TaskManagementTool.UnitTests.BusinessLogic
{
    public class TestTodoService
    {
        private ITodoService todoService;

        [SetUp]
        public void Setup()
        {
            MapperConfiguration config = new(cf => cf.AddProfile(new DefaultMappingProfile()));

            IFixture fixture = new Fixture().Customize(new AutoNSubstituteCustomization());

            ITodoRepository todoRepository = fixture.Create<ITodoRepository>();

            todoRepository.GetSingleAsync(1).Returns(Task.FromResult(SeedData.Todos.First()));
            todoRepository.GetSingleAsync(2).Returns(Task.FromResult(SeedData.Todos.Skip(1).First()));
            todoRepository.GetSingleAsync(3).Returns(Task.FromResult(SeedData.Todos.Skip(2).First()));
            todoRepository.GetSingleAsync(4).Returns(Task.FromResult(SeedData.Todos.Last()));

            todoRepository.GetAsync().Returns(SeedData.Todos);

            todoService = new TodoService(config.CreateMapper(), todoRepository);
        }

        [Test]
        public void GetSingleAsync()
        {
            TodoDto expectedTodo = todoService.GetSingleAsync(1).GetAwaiter().GetResult();
            TodoDto falseTodo = todoService.GetSingleAsync(5).GetAwaiter().GetResult();

            Assert.NotNull(expectedTodo);
            Assert.Null(falseTodo);
            Assert.AreEqual(expectedTodo.Id, 1);
        }

        [Test]
        public void GetAsync()
        {
            ICollection<TodoDto> todos = todoService.GetAsync().GetAwaiter().GetResult();

            Assert.NotNull(todos);
            Assert.AreEqual(todos.Count, 4);
            Assert.AreEqual(todos.First().Id, 1);
            Assert.AreEqual(todos.Last().Id, 4);
        }
    }
}
