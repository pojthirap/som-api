using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.SearchCriteria.type1
{
    public class SearchResultOutboundBasType1<T>
    {
        public Header<T> Header { get; set; }
        public Status Status { get; set; }
    }

    public class Status
    {
        public string Quotation_No { get; set; }
        public string From_Date { get; set; }
        public string To_Date { get; set; }
        public string SO_Status { get; set; }
        public string SO_Msg { get; set; }
    }
    public class Header<T>
    {
        public string Sale_Org { get; set; }
        public string Dist_Channel { get; set; }
        public string Division { get; set; }
        public string SO_Type { get; set; }
        public string SO_No { get; set; }
        public string Doc_Date { get; set; }
        public string Sold_to { get; set; }
        public string Ship_to { get; set; }
        public string PO_Number { get; set; }
        public string Delivery_date { get; set; }
        public string Payment_Term { get; set; }
        public string Inco1 { get; set; }
        public string Inco2 { get; set; }
        public string Sum_Net { get; set; }
        public string Sum_Tax { get; set; }
        public string Sum_Total { get; set; }
        public List<T> Item { get; set; }
    }
}
