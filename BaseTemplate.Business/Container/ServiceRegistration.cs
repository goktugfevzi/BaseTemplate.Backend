using BaseTemplate.Business.Abstractions;
using BaseTemplate.Business.Services;
using BaseTemplate.Business.ValidationRules.FluentValidation;
using BaseTemplate.Dal.Abstractions;
using BaseTemplate.Dal.Conrete;
using BaseTemplate.Schema.Dtos.ExampleDtos;
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
            //services.AddTransient<IValidator<CreateExampleRequest>, ExampleValidator>();
         
        }
    }
}
