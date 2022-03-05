using System.Linq;
using System.Threading.Tasks;
using IntegrationTests.SqlServer.EfCore.Configuration;
using TaskManagementTool.DataAccess.Entities;

namespace IntegrationTests.SqlServer.EfCore.Utils
{
    public static class TestTodoDatabaseUtils
    {
        public static async Task<int> AddTempRecordAndReturnId(string updatedname, string updatedContent = null)
        {
            Todo entity = new()
            {
                Name = updatedname,
                Content = updatedContent
            };

            await TestStartup.Repository.AddAsync(entity);

            int id = (await TestStartup.Repository.GetAsync()).Last().Id;

            return id;
        }

        public static async Task CleanupDatabase(int id) => await TestStartup.Repository.DeleteAsync(id);
    }
}
