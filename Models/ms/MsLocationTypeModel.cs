using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Models.ms
{
    public class MsLocationTypeModel : ModelBase
    {

        public string LocTypeId { get; set; }
        public string LocTypeCode { get; set; }
        public string LocTypeNameTh { get; set; }
        public string LocTypeNameEn { get; set; }
        public string ActiveFlag { get; set; }

    }
}
