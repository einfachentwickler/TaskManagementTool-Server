using Infrastructure.Data.Context;

namespace Infrastructure.Factories;

public interface IDatabaseFactory
{
    public ITaskManagementToolDatabase Create();
}