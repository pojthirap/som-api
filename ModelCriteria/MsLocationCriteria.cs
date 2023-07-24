using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.ModelCriteria
{
    public class MsLocationCriteria
    {
        public string LocId { get; set; }
        public string LocTypeId { get; set; }
        public string LocCode { get; set; }
        public string LocNameTh { get; set; }
        public string LocNameEn { get; set; }
        public string ProvinceCode { get; set; }
        public string ActiveFlag { get; set; }
    }
}
