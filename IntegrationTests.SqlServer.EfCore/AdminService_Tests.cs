using IntegrationTests.SqlServer.EfCore.Utils;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementTool.BusinessLogic.Services;
using TaskManagementTool.BusinessLogic.ViewModels;
using TaskManagementTool.DataAccess.Entities;
using TaskManagementTool.DataAccess.Repositories;
using static IntegrationTests.SqlServer.EfCore.Configuration.TestStartup;

namespace IntegrationTests.SqlServer.EfCore;

[TestFixture]
public class AdminService_Tests
{
    private AdminHandler _instance;

    [SetUp]
    public void Setup()
    {
        _instance = new AdminHandler(Mapper, UserManager, new TodoRepository(DatabaseFactory));
    }

    [Test]
    public async Task GetUsers_Success_ReturnsUsers()
    {
        //Arrange
        string email = $"{Guid.NewGuid()}@example.com";

        await TestUserDatabaseUtils.RegisterTempUserAsync(email, false);

        //Act
        IEnumerable<UserDto> actualResult = await _instance.GetUsersAsync();

        //Assert
        Assert.That(actualResult, Is.Not.Empty);
    }

    [Test]
    public async Task GetUser_Success_User()
    {
        //Arrange
        string email = $"{Guid.NewGuid()}@example.com";
        const bool isBlocked = false;

        await TestUserDatabaseUtils.RegisterTempUserAsync(email, isBlocked);

        User user = await TestUserDatabaseUtils.GetUserAsync(email);

        //Act
        UserDto actualResult = await _instance.GetUserAsync(user.Id);

        //Assert
        Assert.That(actualResult, Is.Not.Null);
        Assert.That(actualResult.Email, Is.Not.Null);
    }

    [TestCase(false)]
    [TestCase(true)]
    public async Task BlockOrUnblockUserAsync_Success_CheckIfBlocked(bool isBlocked)
    {
        //Arrange
        string email = $"{Guid.NewGuid()}@example.com";

        await TestUserDatabaseUtils.RegisterTempUserAsync(email, isBlocked);

        //Act
        User userBeforeUpdate = await TestUserDatabaseUtils.GetUserAsync(email);

        await _instance.BlockOrUnblockUserAsync(userBeforeUpdate.Id);

        User userAfterUpdate = await TestUserDatabaseUtils.GetUserAsync(email);

        //Assert
        Assert.That(userAfterUpdate.IsBlocked, Is.Not.EqualTo(isBlocked));
    }

    [Test]
    public async Task UpdateUserAsync_Success_ReturnsUpdatedUser()
    {
        //Arrange
        string email = $"{Guid.NewGuid()}@example.com";
        const bool isBlocked = false;

        await TestUserDatabaseUtils.RegisterTempUserAsync(email, isBlocked);

        UserDto userToUpdate = await TestUserDatabaseUtils.GetUserDtoAsync(email);

        string firstName = Guid.NewGuid().ToString();

        userToUpdate.FirstName = firstName;

        //Act
        await _instance.UpdateUserAsync(userToUpdate);

        //Assert
        User userAfterUpdate = await TestUserDatabaseUtils.GetUserAsync(email);

        Assert.That(userAfterUpdate, Is.Not.Null);
        Assert.That(userAfterUpdate.FirstName, Is.EqualTo(firstName));
    }

    [Test]
    public async Task DeleteUserAsync_Success_Deletes()
    {
        //Arrange
        string email = $"{Guid.NewGuid()}@example.com";

        await TestUserDatabaseUtils.RegisterTempUserAsync(email, false);

        User user = await TestUserDatabaseUtils.GetUserAsync(email);

        //Act
        await _instance.DeleteUserAsync(user.Id);

        //Assert
        Assert.ThrowsAsync<InvalidOperationException>(async () => await _instance.GetUserAsync(user.Id));
    }
}