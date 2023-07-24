using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Entity.custom
{
    [Keyless]
    public class SearchLocationCustom
    {

        public decimal LocTypeId { get; set; }
        public string LocTypeCode { get; set; }
        public string LocTypeNameTh { get; set; }
        public string LocTypeNameEn { get; set; }


        public decimal LocId { get; set; }
        public string ActiveFlag { get; set; }
        public string LocCode { get; set; }
        public string LocNameTh { get; set; }
        public string LocNameEn { get; set; }
        public string ProvinceCode { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDtm { get; set; }
        public string UpdateUser { get; set; }
        public DateTime UpdateDtm { get; set; }

        public string CountryCode { get; set; }
        public string RegionCode { get; set; }
        public string ProvinceNameTh { get; set; }
        public string ProvinceNameEn { get; set; }



    }
}
