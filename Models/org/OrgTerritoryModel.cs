using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Models.org
{
    public class OrgTerritoryModel : ModelBase
    {

        public string TerritoryId { get; set; }
        public string TerritoryCode { get; set; }
        public string TerritoryNameTh { get; set; }
        public string TerritoryNameEn { get; set; }
        public string ManagerEmpId { get; set; }
        public string ActiveFlag { get; set; }
    }
}
