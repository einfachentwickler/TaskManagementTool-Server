using System.Threading.Tasks;
using TaskManagementTool.BusinessLogic.ViewModels.ToDoModels;
using TaskManagementTool.DataAccess.Entities;

namespace TaskManagementTool.BusinessLogic.Contracts
{
    public interface ICopyService
    {
        public Task<TodoEntry> CopyTodoAsync(UpdateTodoDto todo);

        public Task<User> CopyUserAsync(User todo);
    }
}
