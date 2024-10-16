using BaseTemplate.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BaseTemplate.Repository.Elastic.Abstraction
{
    public interface IElasticService
    {
        Task CreateIndexIfNotExistsAsync(string indexName);
        Task<bool> AddOrUpdateAudit(Audit audit);
        Task<User> Get(string key);
        Task<List<User>?> GetAll();
        Task<bool> Remove(string key);
        Task<long?> RemoveAll();
    }
}