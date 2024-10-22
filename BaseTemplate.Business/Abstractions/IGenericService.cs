using BaseTemplate.Domain.Entities.Common;
using BaseTemplate.Shared.Dtos.SystemDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BaseTemplate.Business.Abstractions
{
    public interface IGenericService<Tentity> where Tentity : BaseEntity
    {
        Task<ServiceResult<List<Tres>>> GetAllAsync<Tres>(bool? tracking = false);
        Task<ServiceResult<List<Tentity>>> GetAllAsync(bool? tracking = false);

        Task<ServiceResult<List<Tres>>> GetAllAsync<Tres>(Expression<Func<Tentity, bool>> expression, bool? tracking = false);
        Task<ServiceResult<List<Tentity>>> GetAllAsync(Expression<Func<Tentity, bool>> expression, bool? tracking, params Expression<Func<Tentity, object>>[] includeExpression);
        Task<ServiceResult<List<Tres>>> GetAllAsync<Tres>(Expression<Func<Tentity, bool>> expression, bool? tracking, params Expression<Func<Tentity, object>>[] includeExpression);
        Task<ServiceResult<PagingResult<Tres>>> GetAllPagingAsync<Tres>(PagingRequest pagingRequest);

        Task<ServiceResult<PagingResult<Tres>>> GetAllPagingAsync<Tres>(Expression<Func<Tentity, bool>> expression, PagingRequest pagingRequest);
        Task<ServiceResult<PagingResult<Tres>>> GetAllPagingAsync<Tres>(Expression<Func<Tentity, bool>>? expression, PagingRequest pagingRequest, params Expression<Func<Tentity, object>>[] includeParams);
        Task<ServiceResult<Tres>> GetByIdAsync<Tres>(string id, bool? tracking = false);
        Task<ServiceResult<Tentity>> GetByIdAsync(string id, bool? tracking = false);
        Task<ServiceResult<Tentity>> GetByIdAsync(string id, bool? tracking = false, params Expression<Func<Tentity, object>>[] includeExpression);
        Task<ServiceResult<Tres>> GetFirstAsync<Tres>(Expression<Func<Tentity, bool>> expression, bool? tracking = false);
        Task<ServiceResult<Tres>> GetFirstAsync<Tres>(Expression<Func<Tentity, bool>> expression, bool? tracking = false, params Expression<Func<Tentity, object>>[] includeExpression);
        Task<ServiceResult<Tentity>> GetFirstAsync(Expression<Func<Tentity, bool>> expression, bool? tracking = false, params Expression<Func<Tentity, object>>[] includeExpression);
        Task<ServiceResult<Tentity>> GetFirstAsync(Expression<Func<Tentity, bool>> expression, bool? tracking = false);
        Task<ServiceResult<Tres>> AddAsync<Treq, Tres>(Treq reqDto, bool? isEmptyResponse = false);
        Task<ServiceResult<List<Tres>>> AddRangeAsync<Treq, Tres>(List<Treq> reqListDto, bool? isEmptyResponse = false);
        Task<ServiceResult<Tres>> UpdateAsync<Treq, Tres>(Treq reqDto, string id, bool? isEmptyResponse = false);

        Task<ServiceResult<List<Tres>>> UpdateRangeAsync<Treq, Tres>(List<Treq> reqDto, bool? isEmptyResponse = false);
        Task<ServiceResult<Tres>> RemoveAsync<Tres>(string id, bool? isEmptyResponse = false);
        Task<ServiceResult<Tres>> SetToPassiveAsync<Tres>(string id, bool? isEmptyResponse = false);

        Task<bool> ExistAsync(Expression<Func<Tentity, bool>> expression);

        Task<bool> ExistAsync(List<Expression<Func<Tentity, bool>>> expressions);


    }
}
