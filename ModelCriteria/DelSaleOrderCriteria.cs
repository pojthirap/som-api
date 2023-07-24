using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.ModelCriteria
{
    public class DelSaleOrderCriteria
    {
        public string typeSubmit { get; set; }
        public string changeTabDesc { get; set; }
        public SaleOrder SaleOrder { get; set; }
        public List<SaleOrderProduct> items { get; set; }
    }
}
