using IntegrationTests.SqlServer.EfCore.Configuration;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementTool.BusinessLogic.Contracts;
using TaskManagementTool.BusinessLogic.Services;
using TaskManagementTool.BusinessLogic.ViewModels;
using TaskManagementTool.BusinessLogic.ViewModels.ToDoModels;

namespace IntegrationTests.SqlServer.EfCore
{
    [TestFixture]
    public class TodoService_Test
    {
        private ITodoService _instance;

        [SetUp]
        public void Setup()
        {
            _instance = new TodoService(TestStartup.Mapper, TestStartup.Repository);
        }

        [Test]
        public async Task GetAsync_Test()
        {
            IEnumerable<TodoDto> actualResult = await _instance.GetAsync();

            Assert.That(actualResult.Any());
        }

        [Test]
        [TestCase(1)]
        public async Task GetSingleAsync_CorrectIdTest(int id)
        {
            TodoDto actualResult = await _instance.GetSingleAsync(id);

            Assert.That(actualResult is not null);
        }

        [Test]
        [TestCase(9999)]
        public void GetSingleAsync_WrongIdTest(int id)
        {
            Assert.ThrowsAsync<InvalidOperationException>(async () => await _instance.GetSingleAsync(id));
        }

        [Test]
        public async Task AddAsync_Test()
        {
            //arrange
            string expectedName = Guid.NewGuid().ToString();

            //act
            CreateTodoDto entity = new()
            {
                Name = expectedName
            };

            await _instance.AddAsync(entity);

            int id = (await _instance.GetAsync()).Last().Id;

            TodoDto actualResult = await _instance.GetSingleAsync(id);

            //assert
            Assert.That(actualResult.Name == expectedName);

            await CleanupDatabase(id);
        }

        [Test]
        public async Task DeleteAsync_Test()
        {
            //arrange
            string expectedName = Guid.NewGuid().ToString();

            int id = await AddTempRecordAndReturnId(expectedName);

            //act
            await _instance.DeleteAsync(id);

            //assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => await _instance.GetSingleAsync(id));
        }

        [Test]
        public async Task UpdateAsync_Test()
        {
            //arrange
            string updatedName = Guid.NewGuid().ToString();
            string updatedContent = Guid.NewGuid().ToString();

            int id = await AddTempRecordAndReturnId(updatedName, updatedContent);

            UpdateTodoDto entityToUpdate = new()
            {
                Id = id,
                Name = updatedName,
                Content = updatedContent
            };

            //act
            await _instance.UpdateAsync(entityToUpdate);

            TodoDto actualresult = await _instance.GetSingleAsync(id);

            //assert
            Assert.That(actualresult.Name == updatedName && actualresult.Content == updatedContent);

            await CleanupDatabase(id);
        }

        private async Task<int> AddTempRecordAndReturnId(string updatedname, string updatedContent = null)
        {
            CreateTodoDto entity = new()
            {
                Name = updatedname, 
                Content = updatedContent
            };
           
            await _instance.AddAsync(entity);
            
            int id = (await _instance.GetAsync()).Last().Id;
            return id;
        }

        private async Task CleanupDatabase(int id) => await _instance.DeleteAsync(id);
    }
}
