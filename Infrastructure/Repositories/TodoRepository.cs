using Infrastructure.Contracts;
using Infrastructure.Data.Context;
using Infrastructure.Data.Entities;
using Infrastructure.Extensions;
using Infrastructure.Factories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Infrastructure.Repositories;

public class TodoRepository(IDatabaseFactory factory) : ITodoRepository
{
    public async Task<IEnumerable<ToDoEntity>> GetAsync(int pageSize, int pageNumber)
    {
        await using ITaskManagementToolDatabase db = factory.Create();

        await db.Database.EnsureCreatedAsync();

        return await db.Todos
            .OrderByDescending(todo => todo.Importance)
            .Page(pageSize, pageNumber)
            .Include(todo => todo.Creator)
            .ToListAsync();
    }

    public async Task<IEnumerable<ToDoEntity>> GetAsync(string userId, int pageSize, int pageNumber)
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

    public async Task<ToDoEntity> FirstOrDefaultAsync(int id)
    {
        await using ITaskManagementToolDatabase db = factory.Create();

        return await db.Todos
            .Include(todo => todo.Creator)
            .FirstOrDefaultAsync(todo => todo.Id == id);
    }

    public async Task<ToDoEntity> CreateAsync(ToDoEntity item)
    {
        await using ITaskManagementToolDatabase db = factory.Create();

        ToDoEntity createdTodo = (await db.Todos.AddAsync(item)).Entity;
        await db.SaveChangesAsync();

        return createdTodo;
    }

    public async Task<ToDoEntity> UpdateAsync(ToDoEntity item)
    {
        await using ITaskManagementToolDatabase db = factory.Create();

        ToDoEntity updatedTodo = db.Todos.Update(item).Entity;
        await db.SaveChangesAsync();

        return updatedTodo;
    }

    public async Task DeleteAsync(Expression<Func<ToDoEntity, bool>> predicate)
    {
        await using ITaskManagementToolDatabase db = factory.Create();

        await db.Todos.Where(predicate).ExecuteDeleteAsync();

        await db.SaveChangesAsync();
    }
}