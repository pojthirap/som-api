using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Models.pospect
{
    public class ProspectDedicateTertModel : ModelBase
    {

        public string ProspDedicateId { get; set; }
        public string ProspectId { get; set; }
        public string[] TerritoryId { get; set; }
        public string ActiveFlag { get; set; }


    }
}
