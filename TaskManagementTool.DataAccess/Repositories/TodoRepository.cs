using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task<ICollection<Todo>> GetAsync()
        {
            ICollection<Todo> todos = await _context.Todos
                .Include(todo => todo.Creator)
                .ToListAsync();

            return todos;
        }

        public async Task<Todo> GetSingleAsync(int id)
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
