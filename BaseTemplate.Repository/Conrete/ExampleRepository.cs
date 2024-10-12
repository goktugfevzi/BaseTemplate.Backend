using BaseTemplate.Repository.Abstractions;
using BaseTemplate.Repository.Contexts;
using BaseTemplate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseTemplate.Repository.Conrete
{
    public class ExampleRepository : GenericRepository<Example>, IExampleRepository
    {
        public ExampleRepository(ExampleContext db) : base(db)
        {
        }
    }
}
