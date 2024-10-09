using BaseTemplate.Dal.Abstractions;
using BaseTemplate.Schema.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseTemplate.Dal.Conrete
{
    internal class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ExampleRepository db) : base(db)
        {

        }

    }
}
