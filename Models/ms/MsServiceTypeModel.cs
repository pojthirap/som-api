using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Models.ms
{
    public class MsServiceTypeModel : ModelBase
    {

        public string ServiceTypeId { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceNameTh { get; set; }
        public string ServiceNameEn { get; set; }
        public string ActiveFlag { get; set; }

    }
}
