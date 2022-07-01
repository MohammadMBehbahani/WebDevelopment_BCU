using System.Collections.Generic;
using System.Linq;

namespace WebDevelopment_BCU.Utility
{
    public static class Pagination
    {
        public static IEnumerable<TSource> ToPages<TSource>(this IEnumerable<TSource> source, int page, int pageSize, out int rowsCount)
        {
            rowsCount = source.Count();

            if (rowsCount < pageSize)
            {
                pageSize = rowsCount;
            }

            return source.Skip((page - 1) * pageSize).Take(pageSize);

        }
    }
}
