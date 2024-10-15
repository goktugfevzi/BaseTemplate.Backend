using Serilog;
using Serilog.Sinks.Elasticsearch;
using Microsoft.Extensions.Configuration;
using System;

namespace BaseTemplate.Presentation.Configurations
{
    public class SerilogConfiguration
    {
        private readonly IConfiguration _configuration;

        public SerilogConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ElasticsearchSinkOptions ElasticsearchOptions
        {
            get
            {
                var uri = _configuration["ElasticsSearchSettings:Url"];
                return new ElasticsearchSinkOptions(new Uri(uri))
                {
                    AutoRegisterTemplate = true,
                    IndexFormat = $"Serilog-{DateTime.UtcNow:yyyy.MM.dd}",
                    NumberOfShards = 2,
                    NumberOfReplicas = 1
                };
            }
        }

        public LoggerConfiguration ConfigureLogging()
        {
            return new LoggerConfiguration()
                .WriteTo.Console(restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
                .WriteTo.Elasticsearch(ElasticsearchOptions)
                .Enrich.FromLogContext()
                .MinimumLevel.Warning();
        }
    }
}
