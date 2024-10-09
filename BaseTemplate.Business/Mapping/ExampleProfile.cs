using AutoMapper;
using BaseTemplate.Entity.Entities;
using BaseTemplate.Shared.Dtos.ExampleDtos;
using BaseTemplate.Shared.Dtos.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseTemplate.Business.Mapping
{
    public class ExampleProfile : Profile
    {
        public ExampleProfile()
        {
            CreateMap<Example, CreateExampleDto>().ReverseMap();
        }
    }
}
