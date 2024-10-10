using AutoMapper;
using BaseTemplate.Schema.Dtos.ExampleDtos;
using BaseTemplate.Schema.Entities;
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
            CreateMap<Example, CreateExampleRequest>().ReverseMap();
        }
    }
}
