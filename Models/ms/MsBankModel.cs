using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Models.ms
{
    public class MsBankModel : ModelBase
    {

        public string BankId { get; set; }
        public string BankCode { get; set; }
        public string BankNameTh { get; set; }
        public string BankNameEn { get; set; }
        public string ActiveFlag { get; set; }

    }
}
