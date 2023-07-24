using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Models.ms
{
    public class MsActtachCategoryModel : ModelBase
    {

        public string AttachCateId { get; set; }
        public string AttachCateCode { get; set; }
        public string AttachCateNameTh { get; set; }
        public string AttachCateNameEn { get; set; }
        public string ActiveFlag { get; set; }

    }
}
