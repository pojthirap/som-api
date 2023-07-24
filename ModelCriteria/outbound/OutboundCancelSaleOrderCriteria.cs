using MyFirstAzureWebApp.SearchCriteria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.ModelCriteria.outbound.cancelsaleorder
{
    public class OutboundCancelSaleOrderCriteria
    {
        public string Delete_Indicator { get; set; }
        public string SAP_Sale_Order { get; set; }
        public string Sale_Org { get; set; }
        public string Distribution_Channel { get; set; }
        public string Division { get; set; }
        public string Sales_Order_Type { get; set; }
        public string SOM_Order_No { get; set; }
        public string Document_Date { get; set; }
    }

}
