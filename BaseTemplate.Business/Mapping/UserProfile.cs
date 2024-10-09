using AutoMapper;
using BaseTemplate.Schema.Entities;
using BaseTemplate.Schema.Dtos.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseTemplate.Business.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserInfoDto>().ReverseMap();
        }
    }
}
