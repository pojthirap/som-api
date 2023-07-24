using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Models.ms
{
    public class MsGasolineModel : ModelBase
    {

        public string GasId { get; set; }
        public string GasNameTh { get; set; }
        public string GasNameEn { get; set; }
        public string ActiveFlag { get; set; }
        public string GasCode { get; set; }

    }
}
