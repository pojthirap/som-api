using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Models.ms
{
    public class MsBrandModel : ModelBase
    {

        public string BrandId { get; set; }
        public string BrandCode { get; set; }
        public string BrandNameTh { get; set; }
        public string BrandNameEn { get; set; }
        public string ActiveFlag { get; set; }

    }
}
