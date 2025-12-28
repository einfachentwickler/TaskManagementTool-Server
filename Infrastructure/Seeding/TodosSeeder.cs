using Infrastructure.Context;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infrastructure.Seeding;

public static class TodosSeeder
{
    public static async Task SeedAsync(ITaskManagementToolDbContext context, string adminEmail)
    {
        var admin = await context.Users.FirstOrDefaultAsync(u => u.Email == adminEmail);

        if (admin == null || await context.Todos.AnyAsync())
            return;

        var todos = new[]
        {
            new ToDoEntity { Name = "Todo 1 init", Content = "Todo 1 content", IsCompleted = false, Importance = 10, Creator = admin, CreatorId = admin.Id },
            new ToDoEntity { Name = "Todo 2 init", Content = "Todo 2 content", IsCompleted = true, Importance = 2, Creator = admin, CreatorId = admin.Id }
        };

        await context.Todos.AddRangeAsync(todos);
        await context.SaveChangesAsync(default);
    }
}