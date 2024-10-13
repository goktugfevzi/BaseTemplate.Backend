using BaseTemplate.Business.Abstractions;
using BaseTemplate.Business.Services;
using BaseTemplate.Business.ValidationRules.FluentValidation;
using BaseTemplate.Repository.Abstractions;
using BaseTemplate.Repository.Conrete;
using BaseTemplate.Domain.Dtos.ExampleDtos;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BaseTemplate.Business.Container
{
    public static class ServiceRegistration
    {
        public static void RegisterBusinessServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMemoryCache();
        }
    }
}
