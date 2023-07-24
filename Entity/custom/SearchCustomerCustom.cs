using Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Entity.custom
{
    [Keyless]
    public class SearchCustomerCustom
    {


        public string CustCode { get; set; }
        public string CustNameTh { get; set; }
        public string CustNameEn { get; set; }
        public string AddressFullnm { get; set; }
    }
}
