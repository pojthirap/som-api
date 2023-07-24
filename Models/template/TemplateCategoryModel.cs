using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Models.ms
{
    public class TemplateCategoryModel : ModelBase
    {

        public string TpCateId { get; set; }
        public string TpCateCode { get; set; }
        public string TpCateNameTh { get; set; }
        public string TpCateNameEn { get; set; }
        public string ActiveFlag { get; set; }

    }
}
