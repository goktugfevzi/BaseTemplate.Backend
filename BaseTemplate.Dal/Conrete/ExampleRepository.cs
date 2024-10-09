using BaseTemplate.Dal.Abstractions;
using BaseTemplate.Dal.Contexts;
using BaseTemplate.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseTemplate.Dal.Conrete
{
    public class ExampleRepository : GenericRepository<Example>, IExampleRepository
    {
        public ExampleRepository(ExampleContext db) : base(db)
        {
        }
    }
}
