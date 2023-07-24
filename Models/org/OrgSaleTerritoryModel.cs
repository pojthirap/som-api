using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Models.org
{
    public class OrgSaleTerritoryModel : ModelBase
    {

        public string SaleTerritoryId { get; set; }
        public string TerritoryId { get; set; }
        public string EmpId { get; set; }
        public string ActiveFlag { get; set; }
    }
}
