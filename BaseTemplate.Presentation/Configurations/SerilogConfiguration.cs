using Serilog;
using Serilog.Sinks.Elasticsearch;
using Microsoft.Extensions.Configuration;
using System;

namespace BaseTemplate.Presentation.Configurations
{
    public static class SerilogConfiguration
    {
        public static ElasticsearchSinkOptions GetElasticsearchOptions(IConfiguration configuration)
        {
            var uri = configuration["ElasticsSearchSettings:Url"];
            return new ElasticsearchSinkOptions(new Uri(uri))
            {
                AutoRegisterTemplate = true,
                IndexFormat = $"Serilog-{DateTime.UtcNow:yyyy.MM.dd}",
                NumberOfShards = 2,
                NumberOfReplicas = 1
            };
        }

        public static LoggerConfiguration ConfigureLogging(IConfiguration configuration)
        {
            return new LoggerConfiguration()
                .WriteTo.Console(restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
                .WriteTo.Elasticsearch(GetElasticsearchOptions(configuration))
                .Enrich.FromLogContext()
                .MinimumLevel.Warning();
        }
    }
}
