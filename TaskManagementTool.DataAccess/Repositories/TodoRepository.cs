using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TaskManagementTool.DataAccess.Contracts;
using TaskManagementTool.DataAccess.DatabaseContext;
using TaskManagementTool.DataAccess.Entities;
using TaskManagementTool.DataAccess.Extensions;
using TaskManagementTool.DataAccess.Factories;

namespace TaskManagementTool.DataAccess.Repositories;

public class TodoRepository(IDatabaseFactory factory) : ITodoRepository
{
    public async Task<IEnumerable<TodoEntry>> GetAsync(int pageSize, int pageNumber)
    {
        await using ITaskManagementToolDatabase db = factory.Create();

        await db.Database.EnsureCreatedAsync();

        return await db.Todos
            .OrderByDescending(todo => todo.Importance)
            .Page(pageSize, pageNumber)
            .Include(todo => todo.Creator)
            .ToListAsync();
    }

    public async Task<IEnumerable<TodoEntry>> GetAsync(string userId, int pageSize, int pageNumber)
    {
        await using ITaskManagementToolDatabase db = factory.Create();

        await db.Database.EnsureCreatedAsync();

        return await db.Todos
            .Where(todo => todo.CreatorId == userId)
            .OrderByDescending(todo => todo.Importance)
            .Page(pageSize, pageNumber)
            .Include(todo => todo.Creator)
            .ToListAsync();
    }

    public async Task<TodoEntry> FirstOrDefaultAsync(int id)
    {
        await using ITaskManagementToolDatabase db = factory.Create();

        return await db.Todos
            .Include(todo => todo.Creator)
            .FirstOrDefaultAsync(todo => todo.Id == id);
    }

    public async Task<TodoEntry> CreateAsync(TodoEntry item)
    {
        await using ITaskManagementToolDatabase db = factory.Create();

        TodoEntry createdTodo = (await db.Todos.AddAsync(item)).Entity;
        await db.SaveChangesAsync();

        return createdTodo;
    }

    public async Task<TodoEntry> UpdateAsync(TodoEntry item)
    {
        await using ITaskManagementToolDatabase db = factory.Create();

        TodoEntry updatedTodo = db.Todos.Update(item).Entity;
        await db.SaveChangesAsync();

        return updatedTodo;
    }

    public async Task DeleteAsync(Expression<Func<TodoEntry, bool>> predicate)
    {
        await using ITaskManagementToolDatabase db = factory.Create();

        await db.Todos.Where(predicate).ExecuteDeleteAsync();

        await db.SaveChangesAsync();
    }
}