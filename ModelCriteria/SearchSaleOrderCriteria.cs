using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.ModelCriteria
{
    public class SearchSaleOrderCriteria
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string CustCode { get; set; }
        public string SomOrderNo { get; set; }
}
}
