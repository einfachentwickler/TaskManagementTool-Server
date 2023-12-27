using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementTool.Common.Enums;
using TaskManagementTool.DataAccess.Contracts;
using TaskManagementTool.DataAccess.DatabaseContext;
using TaskManagementTool.DataAccess.Entities;
using TaskManagementTool.DataAccess.Factories;

namespace TaskManagementTool.DataAccess.Repositories;

public class TodoRepository(IDatabaseFactory factory) : ITodoRepository
{
    private readonly IDatabaseFactory _factory = factory;

    public async Task<IEnumerable<TodoEntry>> GetAsync(SearchCriteriaEnum searchCriteria, string userId = null)
    {
        await using ITaskManagementToolDatabase db = _factory.Create();

        await db.Database.EnsureCreatedAsync();

        if (searchCriteria == SearchCriteriaEnum.GetById)
        {
            if (userId is null)
            {
                throw new NullReferenceException("User id is null");
            }

            return await db.Todos
                .Include(todo => todo.Creator)
                .Where(todo => todo.CreatorId == userId)
                .OrderByDescending(todo => todo.Importance)
                .ToListAsync();
        }

        return await db.Todos
            .Include(todo => todo.Creator)
            .OrderByDescending(todo => todo.Importance)
            .ToListAsync();
    }

    public async Task<TodoEntry> FirstOrDefaultAsync(int id)
    {
        await using ITaskManagementToolDatabase db = _factory.Create();

        return await db.Todos
            .Include(todo => todo.Creator)
            .FirstOrDefaultAsync(todo => todo.Id == id);
    }

    public async Task<TodoEntry> CreateAsync(TodoEntry item)
    {
        await using ITaskManagementToolDatabase db = _factory.Create();

        TodoEntry createdTodo = (await db.Todos.AddAsync(item)).Entity;
        await db.SaveChangesAsync();

        return createdTodo;
    }

    public async Task UpdateAsync(TodoEntry item)
    {
        await using ITaskManagementToolDatabase db = _factory.Create();

        db.Todos.Update(item);
        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await using ITaskManagementToolDatabase db = _factory.Create();

        await db.Todos.Where(x => x.Id == id).ExecuteDeleteAsync();

        await db.SaveChangesAsync();
    }

    public async Task BulkDeleteAsync(string userId)
    {
        //await using ITaskManagementToolDatabase db = _factory.Create();

        //TodoEntry customer = new() { Id = id };
        //db.Todos.e(customer);
        //db.Todos.Remove(customer);

        //await db.SaveChangesAsync();
    }
}