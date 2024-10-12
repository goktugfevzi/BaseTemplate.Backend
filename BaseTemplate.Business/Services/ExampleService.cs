using AutoMapper;
using BaseTemplate.Business.Abstractions;
using BaseTemplate.Business.Cache.Service;
using BaseTemplate.Repository.Abstractions;
using BaseTemplate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseTemplate.Business.Services
{
    public class ExampleService : GenericService<Example>, IExampleService
    {
        public ExampleService(IGenericRepository<Example> genericRepository, IMapper mapper) : base(genericRepository, mapper)
        {
        }
    }
}
