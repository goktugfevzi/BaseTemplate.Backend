using BaseTemplate.Repository.Abstractions;
using BaseTemplate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseTemplate.Repository.Conrete
{
    internal class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ExampleRepository db) : base(db)
        {

        }

    }
}
