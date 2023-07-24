using Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Entity.custom
{
    [Keyless]
    public class SearchSalesTerritoryTabCustom
    {
        public List<OrgTerritory> MyTerritory { get; set; }
        public List<OrgTerritory> DedicatedTerritory { get; set; }
        public List<ObjectSalesRep> SalesRep { get; set; }



        public class ObjectSalesRep
        {
            public AdmEmployee AdmEmployee { get; set; }
            public OrgSaleGroup OrgSaleGroup { get; set; }
            public AdmGroup AdmGroup { get; set; }
            public List<OrgTerritory> listOrgTerritory { get; set; }

        }
    }
}
