using IntegrationTests.SqlServer.EfCore.Configuration;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementTool.BusinessLogic.ViewModels.ToDoModels;
using TaskManagementTool.Common.Enums;
using TaskManagementTool.DataAccess.Entities;

namespace IntegrationTests.SqlServer.EfCore.Utils
{
    public static class TestTodoDatabaseUtils
    {
        public static async Task<int> AddTempRecordAndReturnId(string updatedname, string updatedContent = null)
        {
            TodoEntry entity = new()
            {
                Name = updatedname,
                Content = updatedContent
            };

            await TestStartup.Repository.AddAsync(entity);

            int id = (await TestStartup.Repository.GetAsync(SearchCriteriaEnum.GetAll)).Last().Id;

            return id;
        }

        public static async Task CleanupDatabase(int id) => await TestStartup.Repository.DeleteAsync(id);

        public static CreateTodoDto GetCreateTodoDto(string expectedName)
        {
            CreateTodoDto entity = new()
            {
                Name = expectedName
            };

            return entity;
        }

        public static UpdateTodoDto GetUpdateTodoDto(int id, string updatedName, string updatedContent)
        {
            UpdateTodoDto entityToUpdate = new()
            {
                Id = id,
                Name = updatedName,
                Content = updatedContent
            };

            return entityToUpdate;
        }
    }
}
