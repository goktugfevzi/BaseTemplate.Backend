using BaseTemplate.Repository.Abstractions;
using BaseTemplate.Domain.Entities.Common;
using BaseTemplate.Shared.Dtos.SystemDtos;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BaseTemplate.Repository.Contexts;
using BaseTemplate.Shared.Extensions;

namespace BaseTemplate.Repository.Conrete
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly ExampleContext db;

        public DbSet<T> Table => db.Set<T>();


        public GenericRepository(ExampleContext db)
        {
            this.db = db;
        }

        public virtual async Task<T> AddAsync(T model)
        {
            db.Attach(model);
            await db.AddAsync(model);
            return model;
        }

        public virtual async Task<bool> AddAsync(List<T> models)
        {
            try
            {
                await db.AddRangeAsync(models);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private static IQueryable<T> SetIncludes(Expression<Func<T, object>>[] includeExpression, IQueryable<T> query)
        {
            foreach (var includeItem in includeExpression)
            {
                query = query.Include(includeItem);
            }

            return query;
        }

        public virtual IQueryable<T> GetAll(bool isTrack = true, params Expression<Func<T, object>>[] includeExpression)
        {
            var query = Table.AsQueryable();
            query = SetIncludes(includeExpression, query);
            if (!isTrack)
                query = query.AsNoTracking();
            return query;
        }

        public virtual IQueryable<T> GetAll(Expression<Func<T, bool>>? expression, bool isTrack = true, params Expression<Func<T, object>>[] includeExpression)
        {
            var query = expression is not null ? Table.Where(expression) : Table;
            query = SetIncludes(includeExpression, query);
            if (!isTrack)
                query = query.AsNoTracking();
            return query;
        }

        public virtual async Task<PagingResult<T>> GetAllWithPaginationAsync(int pageNumber = 1, int pageSize = 5, bool isTrack = false, params Expression<Func<T, object>>[] includeExpression)
        {
            return await GetAll(isTrack, includeExpression).ToPagedListAsync(pageNumber, pageSize);
        }

        public virtual async Task<PagingResult<T>> GetAllWithPaginationAsync(Expression<Func<T, bool>> expression, int pageNumber = 1, int pageSize = 5, bool isTrack = true, params Expression<Func<T, object>>[] includeExpression)
        {
            return await GetAll(expression, isTrack, includeExpression).ToPagedListAsync(pageNumber, pageSize);
        }

        public virtual async Task<T> GetByIdAsync(string id, bool isTrack = true, params Expression<Func<T, object>>[] includeExpression)
        {
            var query = Table.AsQueryable();
            query = SetIncludes(includeExpression, query);
            if (!isTrack)
                query = query.AsNoTracking();
            return await query.FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));
        }

        public virtual Task<int> GetCountAsync()
        {
            return Table.CountAsync();
        }

        public virtual Task<int> GetCountAsync(Expression<Func<T, bool>> expression)
        {
            return Table.Where(expression).CountAsync();
        }

        public virtual async Task<T> GetSingleAsync(Expression<Func<T, bool>> expression, bool isTrack = true, params Expression<Func<T, object>>[] includeExpression)
        {
            var query = Table.Where(expression);
            query = SetIncludes(includeExpression, query);
            if (!isTrack)
                query = query.AsNoTracking();
            return await query.FirstOrDefaultAsync();
        }

        public virtual bool Remove(T model)
        {
            EntityEntry<T> entityEntry = Table.Remove(model);
            return entityEntry.State == EntityState.Deleted;
        }

        public virtual bool Remove(List<T> models)
        {
            try
            {
                Table.RemoveRange(models);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public virtual async Task<bool> RemoveAsync(string id)
        {
            T model = await Table.FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));
            return Remove(model);
        }

        public virtual async Task<bool> RemoveAsync(Expression<Func<T, bool>> expression)
        {
            var willRemoveEntities = await Table.Where(expression).ToListAsync();
            return Remove(willRemoveEntities);

        }
        public virtual async Task<T> SetToPassiveAsync(string id)
        {
            var data = await Table.FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));
            data.IsActive = false;
            return data;
        }

        public virtual T Update(T model)
        {
            db.Update(model);
            return model;
        }

        public virtual void Update(List<T> models)
        {
            foreach (T model in models)
            {
                EntityEntry entityEntry = Table.Update(model);
            }
        }



        public async Task<int> SaveAsync()
        {
            try
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var changeCount = await db.SaveChangesAsync();
                        transaction.Commit();
                        return changeCount;
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw new Exception(e.Message);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

    }
}
