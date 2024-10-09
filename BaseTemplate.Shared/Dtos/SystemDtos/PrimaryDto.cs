using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseTemplate.Shared.Dtos.SystemDtos
{
    public class PrimaryDto<T>
    {
        public T Id { get; set; }
    }
}
