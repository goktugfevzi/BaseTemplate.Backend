using BaseTemplate.Repository.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BaseTemplate.Repository.Container
{
    public static class ServiceRegistration
    {
        public static void RegisterRepositoryServices(this IServiceCollection services, IConfiguration configManager)
        {
            services.AddDbContext<ExampleContext>(o => o.UseSqlServer(configManager.GetConnectionString("Sql")));


        }
    }
}
