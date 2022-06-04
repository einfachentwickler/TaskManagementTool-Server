using System.Collections.Generic;
using TaskManagementTool.DataAccess.Entities;

namespace TaskManagementTool.UnitTests.BusinessLogic
{
    public static class SeedData
    {
        public static IEnumerable<TodoEntry> Todos { get; } = new List<TodoEntry>()
        {
            new(){ Id = 1, Content = "Todo seed 1 name", Importance = 5, IsCompleted = true, Name = "Todo seed 1 content"},
            new(){ Id = 2, Content = "Todo seed 1 name", Importance = 1, IsCompleted = false, Name = "Todo seed 2 content"},
            new(){ Id = 3, Content = "Todo seed 1 name", Importance = 10, IsCompleted = false, Name = "Todo seed 3 content"},
            new(){ Id = 4, Content = "Todo seed 1 name", Importance = 4, IsCompleted = true, Name = "Todo seed 4 content"}
        };

        public static IEnumerable<User> Users { get; } = new List<User>()
        {
            new(){ Id = "1", FirstName = "User 1", LastName = "User 1l", Email = "user1@email.com", Role = "user" },
            new(){ Id = "2", FirstName = "User 2", LastName = "User 2l", Email = "user2@email.com", Role = "user" }
        };
    }
}
