using AutoMapper;
using BaseTemplate.Business.Abstractions;
using BaseTemplate.Repository.Abstractions;
using BaseTemplate.Domain.Entities.Common;
using BaseTemplate.Shared.Dtos.SystemDtos;
using BaseTemplate.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BaseTemplate.Business.Services
{
    public class GenericService<T> : IGenericService<T> where T : BaseEntity
    {
        private readonly IGenericRepository<T> genericRepository;
        private readonly IMapper mapper;

        public GenericService(IGenericRepository<T> genericRepository, IMapper mapper)
        {
            this.genericRepository = genericRepository;
            this.mapper = mapper;
        }
        public virtual async Task<ServiceResult<Tres>> AddAsync<Treq, Tres>(Treq reqDto, bool? isEmptyResponse = false)
        {
            try
            {
                var entity = mapper.Map<T>(reqDto);
                var addedEntity = await genericRepository.AddAsync(entity);
                await genericRepository.SaveAsync();
                var result = isEmptyResponse.Value ? Activator.CreateInstance<Tres>() : mapper.Map<Tres>(entity);
                return ServiceResult<Tres>.Success((int)HttpStatusCode.Created, result);
            }
            catch (Exception e)
            {
                return ServiceResult<Tres>.Fail((int)HttpStatusCode.InternalServerError, "Kayıt sırasında bir hata oluştu.Internal Service Error");
            }
        }

        public virtual async Task<ServiceResult<List<Tres>>> AddRangeAsync<Treq, Tres>(List<Treq> reqListDto, bool? isEmptyResponse = false)
        {
            try
            {
                var entities = mapper.Map<List<T>>(reqListDto);
                var addedEntities = await genericRepository.AddAsync(entities);
                await genericRepository.SaveAsync();
                var result = isEmptyResponse.Value ? Activator.CreateInstance<List<Tres>>() : mapper.Map<List<Tres>>(entities);
                return ServiceResult<List<Tres>>.Success((int)HttpStatusCode.Created, result);
            }
            catch (Exception e)
            {
                return ServiceResult<List<Tres>>.Fail((int)HttpStatusCode.InternalServerError, "Kayıt sırasında bir hata oluştu.Internal Service Error");
            }
        }

        public virtual async Task<ServiceResult<List<Tres>>> GetAllAsync<Tres>(bool? tracking = false)
        {
            var entities = await genericRepository.GetAll(tracking.Value).OrderBy(x => x.Id).ToListAsync();
            return ServiceResult<List<Tres>>.Success((int)HttpStatusCode.OK, mapper.Map<List<Tres>>(entities));
        }

        public virtual async Task<ServiceResult<List<T>>> GetAllAsync(bool? tracking = false)
        {
            var entities = await genericRepository.GetAll(tracking.Value).OrderBy(x => x.Id).ToListAsync();
            return ServiceResult<List<T>>.Success((int)HttpStatusCode.OK, entities);
        }

        public virtual async Task<ServiceResult<List<Tres>>> GetAllAsync<Tres>(Expression<Func<T, bool>> expression, bool? tracking = false)
        {
            var entities = await genericRepository.GetAll(expression, tracking.Value).ToListAsync();
            return ServiceResult<List<Tres>>.Success((int)HttpStatusCode.OK, mapper.Map<List<Tres>>(entities));
        }

        public virtual async Task<ServiceResult<List<T>>> GetAllAsync(Expression<Func<T, bool>> expression, bool? tracking, params Expression<Func<T, object>>[] includeExpression)
        {
            var queryable = genericRepository.GetAll(expression, tracking.Value, includeExpression);
            return ServiceResult<List<T>>.Success((int)HttpStatusCode.OK, await queryable.ToListAsync());
        }

        public virtual async Task<ServiceResult<List<Tres>>> GetAllAsync<Tres>(Expression<Func<T, bool>> expression, bool? tracking, params Expression<Func<T, object>>[] includeExpression)
        {
            var queryable = genericRepository.GetAll(expression, tracking.Value, includeExpression);
            return ServiceResult<List<Tres>>.Success((int)HttpStatusCode.OK, mapper.Map<List<Tres>>(await queryable.ToListAsync()));
        }

        public virtual async Task<ServiceResult<PagingResult<Tres>>> GetAllPagingAsync<Tres>(PagingRequest pagingRequest)
        {
            var entities = await genericRepository.GetAll(pagingRequest.IsTracking).ToPagedListAsync(pagingRequest.PageNumber, pagingRequest.PageSize, pagingRequest.OrderByName);
            return ServiceResult<PagingResult<Tres>>.Success((int)HttpStatusCode.OK, mapper.Map<PagingResult<Tres>>(entities));
        }

        public virtual async Task<ServiceResult<PagingResult<Tres>>> GetAllPagingAsync<Tres>(Expression<Func<T, bool>> expression, PagingRequest pagingRequest)
        {
            var entities = await genericRepository.GetAll(expression, pagingRequest.IsTracking).ToPagedListAsync(pagingRequest.PageNumber, pagingRequest.PageSize, pagingRequest.OrderByName);
            return ServiceResult<PagingResult<Tres>>.Success((int)HttpStatusCode.OK, mapper.Map<PagingResult<Tres>>(entities));
        }

        public async Task<ServiceResult<PagingResult<Tres>>> GetAllPagingAsync<Tres>(Expression<Func<T, bool>> expression, PagingRequest pagingRequest, params Expression<Func<T, object>>[] includeParams)
        {
            var entities = await genericRepository.GetAll(expression, pagingRequest.IsTracking, includeParams).ToPagedListAsync(pagingRequest.PageNumber, pagingRequest.PageSize, pagingRequest.OrderByName);
            return ServiceResult<PagingResult<Tres>>.Success((int)HttpStatusCode.OK, mapper.Map<PagingResult<Tres>>(entities));
        }

        public virtual async Task<ServiceResult<Tres>> GetByIdAsync<Tres>(string id, bool? tracking = false)
        {
            var entity = await genericRepository.GetByIdAsync(id, tracking.Value);
            return ServiceResult<Tres>.Success((int)HttpStatusCode.OK, mapper.Map<Tres>(entity));
        }

        public virtual async Task<ServiceResult<T>> GetByIdAsync(string id, bool? tracking = false)
        {
            var entity = await genericRepository.GetByIdAsync(id, tracking.Value);
            return ServiceResult<T>.Success((int)HttpStatusCode.OK, entity);
        }

        public async Task<ServiceResult<T>> GetByIdAsync(string id, bool? tracking = false, params Expression<Func<T, object>>[] includeExpression)
        {
            var entity = await genericRepository.GetByIdAsync(id, tracking.Value, includeExpression);
            return ServiceResult<T>.Success((int)HttpStatusCode.OK, entity);
        }

        public virtual async Task<ServiceResult<Tres>> GetFirstAsync<Tres>(Expression<Func<T, bool>> expression, bool? tracking = false)
        {
            var entity = await genericRepository.GetAll(expression, tracking.Value).FirstOrDefaultAsync();
            return ServiceResult<Tres>.Success((int)HttpStatusCode.OK, mapper.Map<Tres>(entity));
        }

        public virtual async Task<ServiceResult<T>> GetFirstAsync(Expression<Func<T, bool>> expression, bool? tracking = false)
        {
            var entity = await genericRepository.GetSingleAsync(expression, tracking.Value);
            return ServiceResult<T>.Success((int)HttpStatusCode.OK, entity);
        }

        public async Task<ServiceResult<Tres>> GetFirstAsync<Tres>(Expression<Func<T, bool>> expression, bool? tracking = false, params Expression<Func<T, object>>[] includeExpression)
        {
            var entity = await genericRepository.GetSingleAsync(expression, tracking.Value, includeExpression);
            return ServiceResult<Tres>.Success((int)HttpStatusCode.OK, mapper.Map<Tres>(entity));
        }

        public async Task<ServiceResult<T>> GetFirstAsync(Expression<Func<T, bool>> expression, bool? tracking = false, params Expression<Func<T, object>>[] includeExpression)
        {
            var entity = await genericRepository.GetSingleAsync(expression, tracking.Value, includeExpression);
            return ServiceResult<T>.Success((int)HttpStatusCode.OK, entity);
        }

        public virtual async Task<ServiceResult<Tres>> RemoveAsync<Tres>(string id, bool? isEmptyResponse = false)
        {
            try
            {
                var entity = await genericRepository.GetByIdAsync(id);
                genericRepository.Remove(entity);
                await genericRepository.SaveAsync();
                var result = isEmptyResponse.Value ? Activator.CreateInstance<Tres>() : mapper.Map<Tres>(entity);
                return ServiceResult<Tres>.Success((int)HttpStatusCode.OK, result);
            }
            catch (Exception e)
            {
                return ServiceResult<Tres>.Fail((int)HttpStatusCode.InternalServerError, "Silme işlemi sırasında bir hata oluştu. İşlem tamamlanamadı. Internal Service Error");
            }
        }

        public virtual async Task<ServiceResult<Tres>> SetToPassiveAsync<Tres>(string id, bool? isEmptyResponse = false)
        {
            try
            {
                var entity = await genericRepository.SetToPassiveAsync(id);
                await genericRepository.SaveAsync();
                Tres result;
                if (typeof(Tres) == typeof(NoContentDto))
                {
                    result = Activator.CreateInstance<Tres>();
                }
                else
                {
                    result = isEmptyResponse.Value ? Activator.CreateInstance<Tres>() : mapper.Map<Tres>(entity);
                }
                return ServiceResult<Tres>.Success((int)HttpStatusCode.OK, result);
            }
            catch (Exception e)
            {
                return ServiceResult<Tres>.Fail((int)HttpStatusCode.InternalServerError, "Pasife çekme işlemi sırasında bir hata oluştu. İşlem tamamlanamadı. Internal Service Error");
            }
        }

        public virtual async Task<ServiceResult<Tres>> UpdateAsync<Treq, Tres>(Treq reqDto, string id, bool? isEmptyResponse = false)
        {
            try
            {
                var entity = await genericRepository.GetByIdAsync(id, true);
                var mappedEntity = mapper.Map(reqDto, entity);
                genericRepository.Update(mappedEntity);
                await genericRepository.SaveAsync();
                var result = isEmptyResponse.Value ? Activator.CreateInstance<Tres>() : mapper.Map<Tres>(entity);
                return ServiceResult<Tres>.Success((int)HttpStatusCode.OK, result);
            }
            catch (Exception e)
            {
                return ServiceResult<Tres>.Fail((int)HttpStatusCode.InternalServerError, "Güncelleme işlemi sırasında bir hata oluştu. İşlem tamamlanamadı. Internal Service Error");
            }
        }

        public virtual async Task<ServiceResult<List<Tres>>> UpdateRangeAsync<Treq, Tres>(List<Treq> reqDto, bool? isEmptyResponse = false)
        {
            try
            {
                List<T> entityList = new();
                foreach (var item in reqDto)
                {
                    var entity = await genericRepository.GetByIdAsync((item as PrimaryDto<Guid>).Id.ToString());
                    var mappedEntity = mapper.Map(reqDto, entity);
                    entityList.Add(mappedEntity);
                }
                genericRepository.Update(entityList);
                await genericRepository.SaveAsync();
                var result = isEmptyResponse.Value ? Activator.CreateInstance<List<Tres>>() : mapper.Map<List<Tres>>(entityList);
                return ServiceResult<List<Tres>>.Success((int)HttpStatusCode.OK, result);
            }
            catch (Exception e)
            {
                return ServiceResult<List<Tres>>.Fail((int)HttpStatusCode.InternalServerError, "Güncelleme işlemi sırasında bir hata oluştu. İşlem tamamlanamadı. Internal Service Error");
            }
        }
    }
}
