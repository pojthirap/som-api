using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Entity.custom
{
    [Keyless]
    public class OrgSaleAreaCustom
    {
        // OrgSaleArea
        public decimal? AreaId { get; set; }
        public String OrgCode { get; set; }
        public String ChannelCode { get; set; }
        public String DivisionCode { get; set; }
        public String BussAreaCode { get; set; }
        //public decimal? BuId { get; set; }
        public String ActiveFlag { get; set; }


        // OrgSaleOrganization
        public String CompanyCode { get; set; }
        public String OrgNameTh { get; set; }
        public String OrgNameEn { get; set; }
        public String Currency { get; set; }


        // OrgDistChannel
        public String ChannelNameTh { get; set; }
        public String ChannelNameEn { get; set; }

        // OrgDivision
        public String DivisionNameTh { get; set; }
        public String DivisionNameEn { get; set; }


        // OrgBusinessArea
        public String BussAreaNameTh { get; set; }
        public String BussAreaNameEn { get; set; }

        //OrgBusinessUnit
        public String BuNameTh { get; set; }

        public String createUser { get; set; }
        public DateTime? createDtm { get; set; }
        public String updateUser { get; set; }
        public DateTime? updateDtm { get; set; }

    }
}
