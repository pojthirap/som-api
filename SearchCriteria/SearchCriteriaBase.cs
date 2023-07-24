using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.SearchCriteria
{
    public class SearchCriteriaBase<T>
    {
        public int searchOption { get; set; }
        public int searchOrder { get; set; }
        public int startRecord { get; set; }
        public int length { get; set; }
        public int pageNo { get; set; }
        public T model {get;set;}
    }
}
