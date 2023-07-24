using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Models.adm
{
    public class AdmGroupModel : ModelBase
    {
        public string GroupId { get; set; }
        public string GroupCode { get; set; }
        public string GroupNameTh { get; set; }
        public string GroupNameEn { get; set; }
        public string ActiveFlag { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }
}
