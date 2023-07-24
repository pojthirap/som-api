using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Models.org
{
    public class RegionModel : ModelBase
    {

        public string regionCode { get; set; }
        public string regionNameTh { get; set; }
        public string regionNameEn { get; set; }
        public string activeFlag { get; set; }
        public string[] provinceCodeList { get; set; }
    }
}
