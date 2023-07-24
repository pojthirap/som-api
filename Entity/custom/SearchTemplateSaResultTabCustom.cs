using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Entity.custom
{
    [Keyless]
    public class SearchTemplateSaResultTabCustom
    {
        public String TpSaFormId { get; set; }
        public String RecSaFormId { get; set; }
        public String TpNameTh { get; set; }
        public String SaleName { get; set; }
        public DateTime? CreateDtm { get; set; }


    }
}
