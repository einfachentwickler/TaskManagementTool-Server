using TaskManagementTool.DataAccess.Entities;

namespace TaskManagementTool.DataAccess.Initializers
{
    public static class DataSeed
    {
        public static User[] Users { get; } =
        {
            new()
            {
                Email = "admin@example.com",
                IsBlocked = false,
                UserName = "admin@example.com",
                Role = "Admin",
                FirstName = "Admin",
                LastName = "Admin's last name"
            }
        };

        public static Todo[] Todos { get; } =
        {
            new() { Name = "Todo 1 init", Content = "Todo 1 content", IsCompleted = false, Importance = 10, Creator = Users[0] },
            new() { Name = "Todo 2 init", Content = "Todo 2 content", IsCompleted = true, Importance = 2, Creator = Users[0] }
        };
    }
}
