using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Entity.custom
{
    [Keyless]
    public class SearchSubDistrictCustom
    {
        public string SubdistrictCode { get; set; }
        public string DistrictCode { get; set; }
        public string SubdistrictNameTh { get; set; }
        public string SubdistrictNameEn { get; set; }
        public string ActiveFlag { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDtm { get; set; }
        public string UpdateUser { get; set; }
        public DateTime UpdateDtm { get; set; }



        public string ProvinceCode { get; set; }
        public string DistrictNameTh { get; set; }
        public string DistrictNameEn { get; set; }



        public string CountryCode { get; set; }
        public string RegionCode { get; set; }
        public string ProvinceNameTh { get; set; }
        public string ProvinceNameEn { get; set; }

        public string SubdistrictSomId { get; set; }


    }
}
