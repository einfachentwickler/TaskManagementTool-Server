using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IntegrationTests.SqlServer.EfCore.Configuration;
using NUnit.Framework;
using TaskManagementTool.BusinessLogic.Contracts;
using TaskManagementTool.BusinessLogic.Services;
using TaskManagementTool.BusinessLogic.ViewModels;
using TaskManagementTool.BusinessLogic.ViewModels.ToDoModels;
using TaskManagementTool.Common.Exceptions;
using TaskManagementTool.DataAccess.Entities;

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
            ICollection<TodoDto> actualResult = await _instance.GetAsync();

            Assert.That(actualResult.Count > 0);
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
            
            CreateTodoDto request = new() { Name = expectedName };

            //act
            await _instance.AddAsync(request);
            int insertedElementId = (await _instance.GetAsync()).Last().Id;

            TodoDto actualResult = await _instance.GetSingleAsync(insertedElementId);

            //assert
            Assert.That(actualResult.Name == expectedName);

            //cleanup
            await _instance.DeleteAsync(insertedElementId);
        }

        [Test]
        public async Task DeleteAsync_Test()
        {
            //arrange
            string expectedName = Guid.NewGuid().ToString();

            CreateTodoDto entity = new() { Name = expectedName };

            await _instance.AddAsync(entity);
            int id = (await _instance.GetAsync()).Last().Id;

            //act
            await _instance.DeleteAsync(id);

            //assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => await _instance.GetSingleAsync(id));
        }

        [Test]
        public async Task UpdateAsync_Test()
        {
            //cleanup
            string updatedName = Guid.NewGuid().ToString();
            string updatedContent = Guid.NewGuid().ToString();
            
            CreateTodoDto entity = new() { Name = updatedName };

            await _instance.AddAsync(entity);

            int id = (await _instance.GetAsync()).Last().Id;

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

            await _instance.DeleteAsync(id);
        }
    }
}
