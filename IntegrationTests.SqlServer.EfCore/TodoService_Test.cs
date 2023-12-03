using IntegrationTests.SqlServer.EfCore.Configuration;
using IntegrationTests.SqlServer.EfCore.Utils;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementTool.BusinessLogic.Services;
using TaskManagementTool.BusinessLogic.ViewModels;
using TaskManagementTool.BusinessLogic.ViewModels.ToDoModels;
using TaskManagementTool.Common.Enums;

namespace IntegrationTests.SqlServer.EfCore;

[TestFixture]
public class TodoService_Test
{
    private TodoHandler _instance;

    [SetUp]
    public void Setup()
    {
        _instance = new TodoHandler(TestStartup.Mapper, TestStartup.Repository);
    }

    [Test]
    public async Task GetAsync_ValidData_ReturnsTodos()
    {
        //Arrange
        string expectedName = Guid.NewGuid().ToString();

        int id = await TestTodoDatabaseUtils.AddTempRecordAndReturnId(expectedName);

        //Act
        IEnumerable<TodoDto> actualResult = await _instance.GetAsync(SearchCriteriaEnum.GetAll);

        //Assert
        Assert.That(actualResult, Is.Not.Empty);

        await TestTodoDatabaseUtils.CleanupDatabase(id);
    }

    [Test]
    public async Task GetSingleAsync_ValidData_ReturnsTodo()
    {
        //Arrange
        string expectedName = Guid.NewGuid().ToString();

        int id = await TestTodoDatabaseUtils.AddTempRecordAndReturnId(expectedName);

        //Act
        TodoDto actualResult = await _instance.FindByIdAsync(id);

        //Assert
        Assert.That(actualResult, Is.Not.Null);

        await TestTodoDatabaseUtils.CleanupDatabase(id);
    }

    [TestCase(9999)]
    public void GetSingleAsync_WrongId_Throws(int id)
    {
        //Act && Assert
        Assert.ThrowsAsync<InvalidOperationException>(async () => await _instance.FindByIdAsync(id));
    }

    [Test]
    public async Task AddAsync_Success_TodoIdAdded()
    {
        //Arrange
        string expectedName = Guid.NewGuid().ToString();

        //Act
        CreateTodoDto entity = TestTodoDatabaseUtils.GetCreateTodoDto(expectedName);

        await _instance.AddAsync(entity);

        //Assert
        int id = (await _instance.GetAsync(SearchCriteriaEnum.GetAll)).Last().Id;

        TodoDto actualResult = await _instance.FindByIdAsync(id);

        Assert.That(actualResult.Name, Is.EqualTo(expectedName));

        await TestTodoDatabaseUtils.CleanupDatabase(id);
    }

    [Test]
    public async Task DeleteAsync_Success_TodoIsDeleted()
    {
        //Arrange
        string expectedName = Guid.NewGuid().ToString();

        int id = await TestTodoDatabaseUtils.AddTempRecordAndReturnId(expectedName);

        //Act
        await _instance.DeleteAsync(id);

        //Assert
        Assert.ThrowsAsync<InvalidOperationException>(async () => await _instance.FindByIdAsync(id));
    }

    [Test]
    public async Task UpdateAsync_Success_TodoIsUpdated()
    {
        //Arrange
        string updatedName = Guid.NewGuid().ToString();
        string updatedContent = Guid.NewGuid().ToString();

        int id = await TestTodoDatabaseUtils.AddTempRecordAndReturnId(updatedName, updatedContent);

        UpdateTodoDto entityToUpdate = TestTodoDatabaseUtils.GetUpdateTodoDto(id, updatedName, updatedContent);

        //Act
        await _instance.UpdateAsync(entityToUpdate);

        TodoDto actualresult = await _instance.FindByIdAsync(id);

        //Assert
        Assert.That(actualresult.Name, Is.EqualTo(updatedName));
        Assert.That(actualresult.Content, Is.EqualTo(updatedContent));

        await TestTodoDatabaseUtils.CleanupDatabase(id);
    }
}