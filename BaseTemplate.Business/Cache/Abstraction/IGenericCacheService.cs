using BaseTemplate.Entity.Entities.Common;
using BaseTemplate.Shared.Dtos.SystemDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BaseTemplate.Business.Cache.Abstraction
{
    public interface IGenericCacheService<Tentity> where Tentity : BaseEntity
    {
        Task<ServiceResult<List<Tres>>> GetAllAsync<Tres>();
        Task<ServiceResult<List<Tentity>>> GetAllAsync();

        Task<ServiceResult<List<Tres>>> GetAllAsync<Tres>(Expression<Func<Tentity, bool>> expression);

        Task<ServiceResult<PagingResult<Tres>>> GetAllPagingAsync<Tres>(int? pageNumber = 1, int? pageSize = 5);

        Task<ServiceResult<PagingResult<Tres>>> GetAllPagingAsync<Tres>(Expression<Func<Tentity, bool>> expression, int? pageNumber = 1, int? pageSize = 5);
        Task<ServiceResult<Tres>> GetByIdAsync<Tres>(string id);
        Task<ServiceResult<Tentity>> GetByIdAsync(string id);
        Task<ServiceResult<Tres>> GetFirstAsync<Tres>(Expression<Func<Tentity, bool>> expression);

        Task<ServiceResult<Tentity>> GetFirstAsync(Expression<Func<Tentity, bool>> expression);
        Task<ServiceResult<Tres>> AddAsync<Treq, Tres>(Treq reqDto, bool? isEmptyResponse = false);
        Task<ServiceResult<List<Tres>>> AddRangeAsync<Treq, Tres>(List<Treq> reqListDto, bool? isEmptyResponse = false);
        Task<ServiceResult<Tres>> UpdateAsync<Treq, Tres>(Treq reqDto, string id, bool? isEmptyResponse = false);
        Task<ServiceResult<List<Tres>>> UpdateRangeAsync<Treq, Tres>(List<Treq> reqDto, bool? isEmptyResponse = false);
        Task<ServiceResult<Tres>> RemoveAsync<Tres>(string id, bool? isEmptyResponse = false);
        Task<ServiceResult<Tres>> SetToPassiveAsync<Tres>(string id, bool? isEmptyResponse = false);
    }
}
