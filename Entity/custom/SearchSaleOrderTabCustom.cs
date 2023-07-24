using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Entity.custom
{
    [Keyless]
    public class SearchSaleOrderTabCustom
    {
        public string SomOrderNo { get; set; }
        public string Description { get; set; }
        public DateTime? SomOrderDte { get; set; }
        public DateTime? PricingDtm { get; set; }
        public string SapMsg { get; set; }
        public string SaleName { get; set; }
        public string OrderStatus { get; set; }
        public string SapOrderNo { get; set; }
        public string Condition1 { get; set; }

    }
}
