using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.SearchCriteria
{
    public class SearchResultBase<T>
    {
        public int recordStart { get; set; }
        public int recordPerPage { get; set; }
        public int totalRecords { get; set; }
        public int pageNo { get; set; }
        public int totalPages { get; set; }

        public List<T> records { get; set; }
    }
}
