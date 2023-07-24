using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Entity.custom
{
    [Keyless]
    public class SearchCustomerSaleByCustCodeCustom
    {
        public String CustSaleId { get; set; }
        public String OrgNameTh { get; set; }
        public String ChannelNameTh { get; set; }
        public String DivisionNameTh { get; set; }
        public String OrgCode { get; set; }
        public String ChannelCode { get; set; }
        public String DivisionCode { get; set; }
        public String ShippingCond { get; set; }

    }
}
