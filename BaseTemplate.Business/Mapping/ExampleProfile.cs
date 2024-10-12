using AutoMapper;
using BaseTemplate.Domain.Dtos.ExampleDtos;
using BaseTemplate.Domain.Entities;
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
            CreateMap<Example, UpdateExampleRequest>().ReverseMap();
            CreateMap<Example, GetExampleResponse>().ReverseMap();
        }
    }
}
