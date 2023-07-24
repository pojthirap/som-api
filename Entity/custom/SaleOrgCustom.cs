using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Entity.custom
{
    [Keyless]
    public class SaleOrgCustom
    {
        public String companyNameTh { get; set; }
        public String companyNameEN { get; set; }
        public String orgNameTh { get; set; }
        public String orgNameEn { get; set; }

        public String companyCode { get; set; }
        public String orgCode { get; set; }
        public String currency { get; set; }
        public String activeFlag { get; set; }
        public String createUser { get; set; }
        public DateTime? createDtm { get; set; }
        public String updateUser { get; set; }
        public DateTime? updateDtm { get; set; }

    }
}
