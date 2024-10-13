using BaseTemplate.Repository.Abstractions;
using BaseTemplate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseTemplate.Repository.Contexts;

namespace BaseTemplate.Repository.Conrete
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ExampleContext db) : base(db)
        {

        }

    }
}
