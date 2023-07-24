using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Models.ms
{
    public class TemplateSaTitleModel : ModelBase
    {

        public string TpSaTitleId { get; set; }
        public string TpSaFormId { get; set; }
        public string TitleColmNo { get; set; }
        public string TitleNameTh { get; set; }
        public string TitleNameEn { get; set; }
        public string AnsType { get; set; }
        public string AnsValType { get; set; }
        public string AnsLovType { get; set; }

    }
}
