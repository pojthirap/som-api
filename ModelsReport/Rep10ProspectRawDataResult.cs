using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.ModelsReport
{
    public class Rep10ProspectRawDataResult
    {
        public string ProspectId { set; get; }
        public string AccName { set; get; }
        public string ProspectType { set; get; }
        public string Latitude { set; get; }
        public string Longitude { set; get; }
        public string SaleRepId { set; get; }
        public string EmpName { set; get; }
        public string GroupCode { set; get; }
        public string DescriptionTh { set; get; }
        public string CreateDate { set; get; }
        public string Address { set; get; }
        public string ProvinceNameTh { set; get; }
        public string DistrictNameTh { set; get; }
        public string SubdistrictNameTh { set; get; }
    }
}
