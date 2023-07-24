using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Entity.custom
{
    [Keyless]
    public class SearchShipToByCustSaleIdCustom
    {
        public String CustPartnerId { get; set; }
        public String CustCode { get; set; }
        public String CustNameTh { get; set; }

    }
}
