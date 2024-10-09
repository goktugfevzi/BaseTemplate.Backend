using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseTemplate.Shared.Dtos.SystemDtos
{
    public class PagingRequest
    {
        public int PageSize { get; set; } = 5;
        public int PageNumber { get; set; } = 1;
        public string OrderByName { get; set; } = "";
        public bool IsTracking { get; set; } = false;
    }
}
