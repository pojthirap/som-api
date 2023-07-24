using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.SearchCriteria.type3
{
    public class SearchResultOutboundBasType3<T>
    {
        public HeaderType3<T> Header { get; set; }
        public StatusType3 Status { get; set; }
    }

    public class StatusType3
    {
        public string SAP_Sale_Order_No { get; set; }
        public string SO_Status { get; set; }
        public string SO_Message { get; set; }
    }
    public class HeaderType3<T>
    {
        public string SAP_Sale_Order_No { get; set; }
        public string Sale_Org { get; set; }
        public string Distribution_Channel { get; set; }
        public string Division { get; set; }
        public string Sales_Order_Type { get; set; }
        public string SOM_order_no { get; set; }
        public string Document_date { get; set; }
        public List<T> Item { get; set; }
    }


    public class ItemType3
    {
        public string SO_item { get; set; }
        public string Material { get; set; }
        public string Material_Desc { get; set; }
        public List<DocumentType3> Document { get; set; }

    }
    public class DocumentType3
    {
        public string Subsequent_doc_cat { get; set; }
        public string Document_Name { get; set; }
        public string document_no { get; set; }
        public string Created_On { get; set; }
        public string Created_Time { get; set; }
    }
}
