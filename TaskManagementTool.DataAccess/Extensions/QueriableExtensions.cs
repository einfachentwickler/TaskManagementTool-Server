using System.Linq;

namespace TaskManagementTool.DataAccess.Extensions;
public static class QueriableExtensions
{
    public static IQueryable<TType> Page<TType>(this IQueryable<TType> source, int pageSize, int pageNumber) where TType : class
    {
        if (pageSize is > 100 or < 1)
        {
            pageSize = 10;
        }

        if (pageNumber is > 100 or < 1)
        {
            pageNumber = 1;
        }

        return source.Skip((pageNumber - 1) * pageSize).Take(pageSize);
    }
}