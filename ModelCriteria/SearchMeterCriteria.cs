using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.ModelCriteria
{
    public class SearchMeterCriteria
    {
        public string QrCode { get; set; }
        public string DispenserNo { get; set; }
        public string NozzleNo { get; set; }
        public string CustCode { get; set; }
        public string ActiveClag { get; set; }
    }
}
