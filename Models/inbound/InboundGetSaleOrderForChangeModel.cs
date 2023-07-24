using Entity;
using MyFirstAzureWebApp.SearchCriteria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.ModelCriteria.inbound.changeSaleOrder
{
    public class InboundGetSaleOrderForChangeModel : SaleOrder
    {
        public string CustSaleId { get; set; }
        public string CustPartnerId { get; set; }
        

    }

    
}
