namespace TaskManagementTool.DataAccess.Factories
{
    public interface IDatabaseFactory
    {
        public ITaskManagementToolDatabase Create();
    }
}
