using Microsoft.EntityFrameworkCore;

namespace HikeOrganiser.Core.Extensions;

public static class LinqExtensions
{
    public static async Task<(List<T> results, int count)> PageAsync<T>(this IQueryable<T> query, FilterModel filter, CancellationToken token = default)
    {
        int count = await query.CountAsync(token);
        
        if (filter is { Page: not null, PageSize: not null })
        {
            List<T> paged = await query
                .Skip(filter.PageSize.Value * (filter.Page.Value - 1))
                .Take(filter.PageSize.Value)
                .ToListAsync(token);

            return (paged, count);
        }
        
        List<T> results = await query.ToListAsync(token);
        
        return (results, count);
    }
}