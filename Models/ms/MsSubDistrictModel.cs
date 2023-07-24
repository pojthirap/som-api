using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Models.ms
{
    public class MsSubDistrictModel : ModelBase
    {

        public string SubdistrictCode { get; set; }
        public string DistrictCode { get; set; }
        public string SubdistrictNameTh { get; set; }
        public string SubdistrictNameEn { get; set; }
        public string ActiveFlag { get; set; }
        public string SubdistrictSomId { get; set; }
    }
}
