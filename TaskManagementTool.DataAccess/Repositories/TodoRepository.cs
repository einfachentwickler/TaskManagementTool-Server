using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementTool.Common.Enums;
using TaskManagementTool.DataAccess.Contracts;
using TaskManagementTool.DataAccess.Entities;
using TaskManagementTool.DataAccess.Factories;

namespace TaskManagementTool.DataAccess.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly IDatabaseFactory _factory;

        public TodoRepository(IDatabaseFactory factory)
        {
            _factory = factory;
        }

        public async Task<IEnumerable<TodoEntry>> GetAsync(SearchCriteriaEnum searchCriteria, string userId = null)
        {
            await using ITaskManagementToolDatabase db = _factory.Create();

            await db.Database.EnsureCreatedAsync();

            IEnumerable<TodoEntry> todos;

            if (searchCriteria == SearchCriteriaEnum.GetById)
            {
                if (userId is null)
                {
                    throw new NullReferenceException("User id is null");
                }

                todos = await db.Todos
                    .Include(todo => todo.Creator)
                    .Where(todo => todo.CreatorId == userId)
                    .OrderByDescending(todo => todo.Importance)
                    .ToListAsync();

                return todos;
            }

            todos = await db.Todos
                .Include(todo => todo.Creator)
                .OrderByDescending(todo => todo.Importance)
                .ToListAsync();

            return todos;
        }

        public async Task<TodoEntry> FirstAsync(int id)
        {
            await using ITaskManagementToolDatabase db = _factory.Create();

            TodoEntry item = await db.Todos
                .Include(todo => todo.Creator)
                .FirstAsync(todo => todo.Id == id);

            return item;
        }

        public async Task AddAsync(TodoEntry item)
        {
            await using ITaskManagementToolDatabase db = _factory.Create();

            await db.Todos.AddAsync(item);
            await db.SaveChangesAsync();
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

            TodoEntry todoEntry = await db.Todos
                .FirstAsync(todo => todo.Id == id);

            db.Todos.Remove(todoEntry);
            await db.SaveChangesAsync();
        }
    }
}
