using Entity;
using MyFirstAzureWebApp.Entity.custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.SearchCriteria
{
    public class SearchTemplateSaFormCustom
    {

        public string TpCode { get; set; }
        public string TpNameTh { get; set; }
        public string TpNameEn { get; set; }
        public string ActiveFlag { get; set; }
        public decimal MasterTotal { get; set; }
        public decimal TitleTotal { get; set; }
        public decimal TpSaFormId { get; set; }
        public DateTime? UpdateDtm { get; set; }


    }
}
