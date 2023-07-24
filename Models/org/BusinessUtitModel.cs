using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Models.org
{
    public class BusinessUnitModel : ModelBase
    {

        public string BuId { get; set; }
        public string BuCode { get; set; }
        public string BuNameTh { get; set; }
        public string BuNameEn { get; set; }
        public string ActiveFlag { get; set; }
    }
}
