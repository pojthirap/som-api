using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Models.saleorder
{
    public class CancelSaleOrderModel
    {

        public string OrderId { get; set; }
        public string SapOrderNo { get; set; }

    }
}
