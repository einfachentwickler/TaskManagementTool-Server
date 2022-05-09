using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementTool.Common.Enums;
using TaskManagementTool.DataAccess.Contracts;
using TaskManagementTool.DataAccess.Entities;

namespace TaskManagementTool.DataAccess.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly Dao _context;

        public TodoRepository(Dao context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        public async Task<IEnumerable<Todo>> GetAsync(SearchCriteriaEnum searchCriteria, string userId = null)
        {
            IEnumerable<Todo> todos;

            if (searchCriteria == SearchCriteriaEnum.GetById)
            {
                if (userId is null)
                {
                    throw new NullReferenceException("User id is null");
                }

                todos = await _context.Todos
                    .Include(todo => todo.Creator)
                    .Where(todo => todo.CreatorId == userId)
                    .OrderByDescending(todo => todo.Importance)
                    .ToListAsync();

                return todos;
            }

            todos = await _context.Todos
                .Include(todo => todo.Creator)
                .OrderByDescending(todo => todo.Importance)
                .ToListAsync();

            return todos;
        }

        public async Task<Todo> FirstAsync(int id)
        {
            Todo item = await _context.Todos
                .Include(todo => todo.Creator)
                .FirstAsync(todo => todo.Id == id);

            return item;
        }

        public async Task AddAsync(Todo item)
        {
            await _context.Todos.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Todo item)
        {
            _context.Todos.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            Todo todo = await _context.Todos
                .FirstAsync(todo => todo.Id == id);

            _context.Todos.Remove(todo);
            await _context.SaveChangesAsync();
        }
    }
}
