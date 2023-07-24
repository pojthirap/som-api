using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Models.org
{
    public class OrgSaleGroupModel : ModelBase
    {

        public string GroupCode { get; set; }
        public string OfficeCode { get; set; }
        public string DescriptionTh { get; set; }
        public string DescriptionEn { get; set; }
        public string ManagerEmpId { get; set; }
        public string ApproveEmpId { get; set; }
        public string ActiveFlag { get; set; }


    }
}
