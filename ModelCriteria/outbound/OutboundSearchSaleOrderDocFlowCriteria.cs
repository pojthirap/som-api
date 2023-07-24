using MyFirstAzureWebApp.SearchCriteria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.ModelCriteria.outbound.searchorderdocflow
{
    public class OutboundSearchSaleOrderDocFlowCriteria
    {
        public Input Input { get; set; }
    }

    public class Input
    {
        public string SAP_Sale_Order { get; set; }
        public string Sale_Org { get; set; }
        public string Distribution_Channel { get; set; }
        public string Division { get; set; }
        public string Sales_Order_Type { get; set; }
        public string SOM_order_no { get; set; }
        public string Document_Date { get; set; }
    }
}
