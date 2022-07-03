using System.Collections.Generic;

namespace WebDevelopment_BCU.Utility
{
    public class ResultPagination<T>
    {
        public List<T> ListData { get; set; }
        public decimal Rows { get; set; }
        public decimal PageSize { get; set; }
        public decimal CurrentPage { get; set; }
        public long TotalCount { get; set; }
        public decimal NumberOfPage { get; set; }
    }
}
