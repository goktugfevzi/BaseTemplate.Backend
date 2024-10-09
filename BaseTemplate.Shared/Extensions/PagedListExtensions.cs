using BaseTemplate.Shared.Dtos.SystemDtos;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseTemplate.Shared.Extensions
{
    public static class PagedListExtensions
    {
        public static async Task<PagingResult<T>> ToPagedListAsync<T>(this IQueryable<T> source, int pageNumber, int pageSize, string orderByName = null)
        {
            var count = await source.CountAsync();
            List<T> items;
            if (string.IsNullOrEmpty(orderByName))
                items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            else
                items = await source.OrderBy(orderByName).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return new(items, pageNumber, pageSize, count);
        }

        public static async Task<PagingResult<T>> ToPagedListAsync<T>(this IOrderedQueryable<T> source, int pageNumber, int pageSize, string orderByName = null)
        {
            var count = await source.CountAsync();
            List<T> items;
            if (string.IsNullOrEmpty(orderByName))
                items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            else
                items = await source.OrderBy(orderByName).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return new(items, pageNumber, pageSize, count);
        }
        public static PagingResult<T> ToPagedList<T>(this IQueryable<T> source, int pageNumber, int pageSize, string orderByName = null)
        {
            var count = source.Count();
            List<T> items;
            if (string.IsNullOrEmpty(orderByName))
                items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            else
                items = source.AsQueryable().OrderBy(orderByName).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new(items, pageNumber, pageSize, count);
        }
    }
}
