﻿using BaseTemplate.Domain.Entities;
using BaseTemplate.Repository.Elastic.Abstraction;
using BaseTemplate.Repository.Elastic.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Nest;

namespace BaseTemplate.Repository.Elastic.Service
{
    public class ElasticService : IElasticService
    {
        private readonly IElasticClient _client;
        private readonly ElasticSettings _elasticSettings;
        private readonly IConfiguration _configuration;


        public ElasticService(IOptions<ElasticSettings> elasticSettings, IConfiguration configuration)
        {
            _elasticSettings = elasticSettings.Value;
            _configuration = configuration;
            _elasticSettings.Url = _configuration["ElasticsSearchSettings:Url"];
            _elasticSettings.DefaultIndex = _configuration["ElasticsSearchSettings:DefaultIndex"];

            var settings = new ConnectionSettings(new Uri(_elasticSettings.Url))
                .DefaultIndex(_elasticSettings.DefaultIndex);

            _client = new ElasticClient(settings);
            _configuration = configuration;
        }

        public async Task<bool> AddOrUpdateAudit(Audit audit)
        {
            var response = await _client.IndexDocumentAsync(audit);
            return response.IsValid;
        }
        public async Task CreateIndexIfNotExistsAsync(string indexName)
        {
            var existsResponse = await _client.Indices.ExistsAsync(indexName);

            if (!existsResponse.Exists)
            {
                await _client.Indices.CreateAsync(indexName, c => c
                    .Map<User>(m => m.AutoMap()));
            }
        }

        public async Task<User> Get(string key)
        {
            var response = await _client.GetAsync<User>(key, idx => idx.Index(_elasticSettings.DefaultIndex));
            return response.Source;
        }

        public async Task<List<User>> GetAll()
        {
            var searchResponse = await _client.SearchAsync<User>(s => s
                .Index(_elasticSettings.DefaultIndex)
                .MatchAll());

            return searchResponse.IsValid ? searchResponse.Documents.ToList() : default;
        }

        public async Task<bool> Remove(string key)
        {
            var deleteResponse = await _client.DeleteAsync<User>(key, idx => idx.Index(_elasticSettings.DefaultIndex));
            return deleteResponse.IsValid;
        }

        public async Task<long?> RemoveAll()
        {
            var deleteByQueryResponse = await _client.DeleteByQueryAsync<User>(q => q
                .Index(_elasticSettings.DefaultIndex)
                .Query(q => q.MatchAll()));

            return deleteByQueryResponse.IsValid ? deleteByQueryResponse.Deleted : default;
        }
    }
}