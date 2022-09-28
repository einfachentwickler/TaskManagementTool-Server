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
using TaskManagementTool.Common.Enums;
using TaskManagementTool.DataAccess.Contracts;
using TaskManagementTool.Host.Configuration.Profiles;

namespace TaskManagementTool.UnitTests.BusinessLogic
{
    public class TodoService_Test
    {
        private ITodoService todoService;

        [SetUp]
        public void Setup()
        {
            DefaultMappingProfile defaultMappingProfile = new();
            AutoNSubstituteCustomization autoNSubstituteCustomization = new();

            void AddProfile(IMapperConfigurationExpression configuration) => configuration.AddProfile(defaultMappingProfile);

            MapperConfiguration config = new(AddProfile);

            IMapper mapper = config.CreateMapper();

            IFixture fixture = new Fixture().Customize(autoNSubstituteCustomization);

            ITodoRepository todoRepository = fixture.Create<ITodoRepository>();

            todoRepository.FirstAsync(1).Returns(Task.FromResult(SeedData.Todos.First()));
            todoRepository.FirstAsync(2).Returns(Task.FromResult(SeedData.Todos.Skip(1).First()));
            todoRepository.FirstAsync(3).Returns(Task.FromResult(SeedData.Todos.Skip(2).First()));
            todoRepository.FirstAsync(4).Returns(Task.FromResult(SeedData.Todos.Last()));

            todoRepository.GetAsync(SearchCriteriaEnum.GetAll).Returns(SeedData.Todos);

            todoService = new TodoService(mapper, todoRepository);
        }

        [Test]
        public async Task GetSingleAsync_Test()
        {
            TodoDto expectedTodo = await todoService.FindByIdAsync(1);
            TodoDto falseTodo = await todoService.FindByIdAsync(5);

            Assert.NotNull(expectedTodo);
            Assert.Null(falseTodo);
            Assert.AreEqual(expectedTodo.Id, 1);
        }

        [Test]
        public async Task GetAsync_Test()
        {
            IEnumerable<TodoDto> todos = await todoService.GetAsync(SearchCriteriaEnum.GetAll);

            Assert.NotNull(todos);

            List<TodoDto> todoDtos = todos.ToList();
            Assert.AreEqual(todoDtos.Count, 4);
            Assert.AreEqual(todoDtos.First().Id, 1);
            Assert.AreEqual(todoDtos.Last().Id, 4);
        }
    }
}
