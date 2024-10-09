using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseTemplate.Shared.Dtos.SystemDtos
{
    public class PagingResult<T>
    {
        public PagingResult()
        {

        }
        public PagingResult(IList<T> datas, int pageNumber, int pageSize, int count)
        {
            Datas = datas;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalPagesCount = (int)Math.Ceiling(count / (double)pageSize);
            IsFirstPage = PageNumber == 1;
            IsLastPage = PageNumber == TotalPagesCount;
        }
        public IList<T> Datas { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPagesCount { get; set; }
        public bool IsFirstPage { get; set; }
        public bool IsLastPage { get; set; }
    }
}
