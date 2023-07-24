using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Entity.custom
{
    [Keyless]
    public class SearchSurveyResultTabCustom
    {
        public String SaleName { get; set; }
        public String TpNameTh { get; set; }
        public DateTime? CreateDtm { get; set; }
        public String RecAppFormId { get; set; }


    }
}
