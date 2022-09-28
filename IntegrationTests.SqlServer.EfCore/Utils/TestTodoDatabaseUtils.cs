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
        public static async Task<int> AddTempRecordAndReturnId(string updatedName, string updatedContent = null)
        {
            TodoEntry entity = new()
            {
                Name = updatedName,
                Content = updatedContent
            };

            await TestStartup.Repository.AddAsync(entity);

            int id = (await TestStartup.Repository.GetAsync(SearchCriteriaEnum.GetAll)).Last().Id;

            return id;
        }

        public static async Task CleanupDatabase(int id) => await TestStartup.Repository.DeleteAsync(id);

        public static CreateTodoDto GetCreateTodoDto(string expectedName)
        {
            return new CreateTodoDto
            {
                Name = expectedName
            };
        }

        public static UpdateTodoDto GetUpdateTodoDto(int id, string updatedName, string updatedContent)
        {
            return new UpdateTodoDto
            {
                Id = id,
                Name = updatedName,
                Content = updatedContent
            };
        }
    }
}
