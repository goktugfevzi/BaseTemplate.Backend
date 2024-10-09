using BaseTemplate.Entity.Entities.Common;
using BaseTemplate.Shared.Dtos.SystemDtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BaseTemplate.Dal.Abstractions
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        public DbSet<T> Table { get; }
        Task<PagingResult<T>> GetAllWithPaginationAsync(int pageNumber = 1, int pageSize = 5, bool isTrack = false, params Expression<Func<T, object>>[] includeExpression);
        Task<PagingResult<T>> GetAllWithPaginationAsync(Expression<Func<T, bool>> expression, int pageNumber = 1, int pageSize = 5, bool isTrack = true, params Expression<Func<T, object>>[] includeExpression);
        IQueryable<T> GetAll(bool isTrack = true, params Expression<Func<T, object>>[] includeExpression);
        IQueryable<T> GetAll(Expression<Func<T, bool>>? expression, bool isTrack = true, params Expression<Func<T, object>>[] includeExpression);
        Task<T> GetSingleAsync(Expression<Func<T, bool>> expression, bool isTrack = true, params Expression<Func<T, object>>[] includeExpression);
        Task<T> GetByIdAsync(string id, bool isTrack = true, params Expression<Func<T, object>>[] includeExpression);
        Task<int> GetCountAsync();
        Task<int> GetCountAsync(Expression<Func<T, bool>> expression);

        Task<T> AddAsync(T model);
        Task<bool> AddAsync(List<T> models);

        T Update(T model);
        void Update(List<T> models);
        Task<T> SetToPassiveAsync(string id);
        bool Remove(T model);
        bool Remove(List<T> models);
        Task<bool> RemoveAsync(string id);
        Task<bool> RemoveAsync(Expression<Func<T, bool>> expression);

        Task<int> SaveAsync();
    }
}
