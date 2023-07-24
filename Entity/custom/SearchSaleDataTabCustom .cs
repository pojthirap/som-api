using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Entity.custom
{
    [Keyless]
    public class SearchSaleDataTabCustom
    {

        public string OrgNameTh { get; set; }
        public string ChannelNameTh { get; set; }
        public string DivisionNameTh { get; set; }
        public string OfficeNameTh { get; set; }
        public string GroupNameTh { get; set; }
        public string CustGroup { get; set; }
        public string PaymentTerm { get; set; }
        public string Incoterm { get; set; }



    }
}
