using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Models.adm
{
    public class AdmLogLoginModel
    {

        public decimal LogLoginId { get; set; }
        public string UserName { get; set; }
        public DateTime LoginDtm { get; set; }
        public string Status { get; set; }
        public string ErrorDescription { get; set; }
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }

    }
}
