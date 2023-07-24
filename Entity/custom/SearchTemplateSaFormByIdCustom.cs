using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Entity.custom
{
    [Keyless]
    public class SearchTemplateSaFormByIdCustom
    {


        public decimal? TpSaTitleId { get; set; }
        public string TitleNameTh { get; set; }
        public string TitleType { get; set; }
        public decimal? AnsLovType { get; set; }
        public decimal? AnsValType { get; set; }
        public string LovNameTh { get; set; }
        public string LovNameEn { get; set; }

    }
}
