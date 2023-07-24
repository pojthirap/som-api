using Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Entity.custom
{
    [Keyless]
    public class SearchSaleOrderCustom : SaleOrder
    {
        public string DocTypeNameTh { get; set; }
        public string Status { get; set; }
        public string CustNameTh { get; set; }

    }
}
