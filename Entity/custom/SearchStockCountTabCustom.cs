using Entity;
using MyFirstAzureWebApp.Entity.custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.SearchCriteria
{
    public class SearchStockCountTabCustom
    {
       
        public class ResponseData
        {

            public string productCode { get; set; }
            public string productName { get; set; }
            public string baseUnit { get; set; }
            public List<RecQty> listRecQty { get; set; }
        }
        public class RecQty
        {
            public string qry { get; set; }
            public string altUnit { get; set; }
        }


        // Temp Class


        public class objectResult
        {
            public string productCode { get; set; }
            public string productName { get; set; }
            public string baseUnit { get; set; }
            public string qry { get; set; }
            public string altUnit { get; set; }

        }


    }
}
