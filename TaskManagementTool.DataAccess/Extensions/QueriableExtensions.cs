using System.Linq;

namespace TaskManagementTool.DataAccess.Extensions;
public static class QueriableExtensions
{
    public static IQueryable<TType> Page<TType>(this IQueryable<TType> source, int pageSize, int pageNumber) where TType : class
    {
        if (pageSize > 1000)
        {
            pageSize = 1000;
        }

        return source.Skip((pageNumber - 1) * pageSize).Take(pageSize);
    }
}