using MyFirstAzureWebApp.SearchCriteria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.ModelCriteria.inbound.changeSaleOrder
{
    public class InboundChangeSaleOrderModelResponse : InboundChangeSaleOrderModel
    {
        public StatusSOM_Change StatusSOM { get; set; }
    }
    public class StatusSOM_Change
    {
        public string SOM_Status { get; set; }
        public string SOM_Message { get; set; }
    }
}
