using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Models.ms
{
    public class MsBrandCategoryModel : ModelBase
    {

        public string BrandCateId { get; set; }
        public string BrandCateCode { get; set; }
        public string BrandCateNameTh { get; set; }
        public string BrandCateNameEn { get; set; }
        public string ActiveFlag { get; set; }

    }
}
