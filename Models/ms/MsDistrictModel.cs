using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Models.ms
{
    public class MsDistrictModel : ModelBase
    {

        public string DistrictCode { get; set; }
        public string ProvinceCode { get; set; }
        public string DistrictNameTh { get; set; }
        public string DistrictNameEn { get; set; }
        public string ActiveFlag { get; set; }

    }
}
