using Entity;
using MyFirstAzureWebApp.SearchCriteria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.ModelCriteria.inbound.cancelSaleOrder
{
    public class InboundCancelSaleOrderModelResponse : InboundCancelSaleOrderModel
    {
        public StatusSOM_Cancel StatusSOM { get; set; }
    }
    public class StatusSOM_Cancel
    {
        public string SOM_Status { get; set; }
        public string SOM_Message { get; set; }
    }

}
