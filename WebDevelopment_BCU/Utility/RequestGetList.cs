using System;

namespace WebDevelopment_BCU.Utility
{
    public class RequestGetList
    {
        public string SearchKey { get; set; }

        public int? Page { get; set; }
        public int? PageSize { get; set; }
    }
}
