using Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Entity.custom
{
    [Keyless]
    public class GetSaleOrderByOrderIdCustom
    {
        public SaleOrder SaleOrder { get; set; }
        public List<SaleOrderProduct> Items { get; set; }
        public List<OrderLogsCustom> OrderLogs { get; set; }


        public class OrderLogsCustom
        {
            public string OrderAction { get; set; }
            public string SomOrderNo { get; set; }
            public string SapOrderNo { get; set; }
            public string SapStatus { get; set; }
            public string SapMsg { get; set; }
        }
    }
}
