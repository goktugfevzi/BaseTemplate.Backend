using AutoMapper;
using BaseTemplate.Business.Abstractions;
using BaseTemplate.Business.Cache.Abstraction;
using BaseTemplate.Repository.Abstractions;
using BaseTemplate.Domain.Entities.Common;
using BaseTemplate.Shared.Dtos.SystemDtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BaseTemplate.Business.Cache.Service
{
    public class GenericCacheService<T> : IGenericCacheService<T> where T : BaseEntity, new()
    {
        private readonly string cacheKey;
        private readonly IGenericRepository<T> genericRepository;
        private readonly IMapper mapper;
        private readonly IMemoryCache _memoryCache;
        private readonly IGenericService<T> genericService;

        public GenericCacheService(IGenericRepository<T> genericRepository, IMapper mapper, IMemoryCache memoryCache, IGenericService<T> genericService)
        {
            this.genericRepository = genericRepository;
            this.genericService = genericService;
            this.mapper = mapper;
            _memoryCache = memoryCache;
            cacheKey = typeof(T).Name + "CacheKey";

            if (!memoryCache.TryGetValue(cacheKey, out _))
            {
                _memoryCache.Set(cacheKey, genericRepository.GetAll(false).ToList());
            }

        }
        public virtual async Task<ServiceResult<Tres>> AddAsync<Treq, Tres>(Treq reqDto, bool? isEmptyResponse = false)
        {
            var result = await genericService.AddAsync<Treq, Tres>(reqDto, isEmptyResponse);
            await RefreshCache();
            return result;
        }

        public virtual async Task<ServiceResult<List<Tres>>> AddRangeAsync<Treq, Tres>(List<Treq> reqListDto, bool? isEmptyResponse = false)
        {
            var results = await genericService.AddRangeAsync<Treq, Tres>(reqListDto, isEmptyResponse);
            await RefreshCache();
            return results;
        }

        public virtual Task<ServiceResult<List<Tres>>> GetAllAsync<Tres>()
        {
            var results = _memoryCache.Get<List<T>>(cacheKey);
            var convertedResults = mapper.Map<List<Tres>>(results);
            return Task.FromResult(ServiceResult<List<Tres>>.Success(200, convertedResults));
        }

        public virtual Task<ServiceResult<List<T>>> GetAllAsync()
        {
            return Task.FromResult(ServiceResult<List<T>>.Success(200, _memoryCache.Get<List<T>>(cacheKey)));
        }

        public virtual Task<ServiceResult<List<Tres>>> GetAllAsync<Tres>(Expression<Func<T, bool>> expression)
        {
            var results = _memoryCache.Get<List<T>>(cacheKey).Where(expression.Compile()).ToList();
            var convertedResults = mapper.Map<List<Tres>>(results);
            return Task.FromResult(ServiceResult<List<Tres>>.Success(200, convertedResults));
        }



        public virtual Task<ServiceResult<PagingResult<Tres>>> GetAllPagingAsync<Tres>(int? pageNumber = 1, int? pageSize = 5)
        {
            var results = _memoryCache.Get<List<T>>(cacheKey).Skip((pageNumber.Value - 1)).Take(pageSize.Value).ToList();
            var convertedResults = mapper.Map<List<Tres>>(results);
            var pagingResult = new PagingResult<Tres>(convertedResults, pageNumber.Value, pageSize.Value, _memoryCache.Get<List<T>>(cacheKey).Count());
            return Task.FromResult(ServiceResult<PagingResult<Tres>>.Success(200, pagingResult));
        }

        public virtual Task<ServiceResult<PagingResult<Tres>>> GetAllPagingAsync<Tres>(Expression<Func<T, bool>> expression, int? pageNumber = 1, int? pageSize = 5)
        {
            var results = _memoryCache.Get<List<T>>(cacheKey).Skip((pageNumber.Value - 1)).Take(pageSize.Value).ToList();
            var convertedResults = mapper.Map<List<Tres>>(results);
            var pagingResult = new PagingResult<Tres>(convertedResults, pageNumber.Value, pageSize.Value, _memoryCache.Get<List<T>>(cacheKey).Count());
            return Task.FromResult(ServiceResult<PagingResult<Tres>>.Success(200, pagingResult));
        }

        public virtual Task<ServiceResult<Tres>> GetByIdAsync<Tres>(string id)
        {
            var result = _memoryCache.Get<List<T>>(cacheKey).Where(x => x.Id == Guid.Parse(id)).FirstOrDefault();
            var convertedResult = mapper.Map<Tres>(result);
            return Task.FromResult(ServiceResult<Tres>.Success(200, convertedResult));
        }

        public virtual Task<ServiceResult<T>> GetByIdAsync(string id)
        {
            return Task.FromResult(ServiceResult<T>.Success(200, _memoryCache.Get<List<T>>(cacheKey).Where(x => x.Id == Guid.Parse(id)).FirstOrDefault()));
        }

        public virtual Task<ServiceResult<Tres>> GetFirstAsync<Tres>(Expression<Func<T, bool>> expression)
        {
            var result = _memoryCache.Get<List<T>>(cacheKey).Where(expression.Compile()).FirstOrDefault();
            var convertedResult = mapper.Map<Tres>(result);
            return Task.FromResult(ServiceResult<Tres>.Success(200, convertedResult));
        }

        public virtual Task<ServiceResult<T>> GetFirstAsync(Expression<Func<T, bool>> expression)
        {
            return Task.FromResult(ServiceResult<T>.Success(200, _memoryCache.Get<List<T>>(cacheKey).Where(expression.Compile()).FirstOrDefault()));
        }

        public virtual async Task<ServiceResult<Tres>> RemoveAsync<Tres>(string id, bool? isEmptyResponse = false)
        {
            var result = await genericService.RemoveAsync<Tres>(id, isEmptyResponse);
            await RefreshCache();
            return result;

        }

        public async virtual Task<ServiceResult<Tres>> SetToPassiveAsync<Tres>(string id, bool? isEmptyResponse = false)
        {
            var result = await genericService.SetToPassiveAsync<Tres>(id, isEmptyResponse);
            await RefreshCache();
            return result;
        }

        public async virtual Task<ServiceResult<Tres>> UpdateAsync<Treq, Tres>(Treq reqDto, string id, bool? isEmptyResponse = false)
        {
            var result = await genericService.UpdateAsync<Treq, Tres>(reqDto, id, isEmptyResponse);
            await RefreshCache();
            return result;
        }

        public async virtual Task<ServiceResult<List<Tres>>> UpdateRangeAsync<Treq, Tres>(List<Treq> reqDto, bool? isEmptyResponse = false)
        {
            var results = await genericService.UpdateRangeAsync<Treq, Tres>(reqDto, isEmptyResponse);
            await RefreshCache();
            return results;
        }

        public async Task RefreshCache()
        {
            _memoryCache.Set(cacheKey, await genericRepository.GetAll().ToListAsync());
        }
    }
}
