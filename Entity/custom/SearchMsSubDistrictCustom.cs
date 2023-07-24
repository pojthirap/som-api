using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Entity.custom
{
    [Keyless]
    public class SearchMsSubDistrictCustom
    {
        public string SubdistrictCode { get; set; }
        public string SubdistrictNameTh { get; set; }
        public string SubdistrictNameEn { get; set; }
        public string ActiveFlag { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDtm { get; set; }
        public string UpdateUser { get; set; }
        public DateTime UpdateDtm { get; set; }



    }
}
