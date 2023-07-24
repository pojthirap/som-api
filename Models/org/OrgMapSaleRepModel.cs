using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Models.org
{
    public class OrgMapSaleRepModel : ModelBase
    {

        public string TerritoryId { get; set; }
        public List<string> EmpId { get; set; }
    }
}
