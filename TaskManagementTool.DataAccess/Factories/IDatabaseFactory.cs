using TaskManagementTool.DataAccess.DatabaseContext;

namespace TaskManagementTool.DataAccess.Factories;

public interface IDatabaseFactory
{
    public ITaskManagementToolDatabase Create();
}