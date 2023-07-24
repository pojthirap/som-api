using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Models.profile
{
    public class UserProfile
    {

        public AdmEmployee AdmEmployee { get; set; }
        public AdmGroup AdmGroup { get; set; }
        public OrgBusinessUnit OrgBusinessUnit { get; set; }
        public OrgSaleGroup OrgSaleGroup { get; set; }
        public OrgSaleOffice OrgSaleOffice { get; set; }
        public List<OrgSaleArea> OrgSaleAreas { get; set; }
        public List<OrgTerritory> OrgTerritorys { get; set; }
    }
}
