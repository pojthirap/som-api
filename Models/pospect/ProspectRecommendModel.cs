using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Models.pospect
{
    public class ProspectRecommendModel : ModelBase
    {

        public string ProspRecommId { get; set; }
        public string ProspectId { get; set; }
        //public string BuId { get; set; }
        public string ActiveFlag { get; set; }
        public string[] BuIdList { get; set; }


    }
}
