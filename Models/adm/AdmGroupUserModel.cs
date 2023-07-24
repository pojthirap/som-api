using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Models.adm
{
    public class AdmGroupUserModel : ModelBase
    {
        public List<EmployeeGroup> EmployeeGroups { get; set; }
        public string GroupId { get; set; }
        public string GroupUserType { get; set; }
        public string ActiveFlag { get; set; }
        public string EffectiveDate { get; set; }
        public string? ExpiryDate { get; set; }
        public string? BuId { get; set; }


    }
    public class EmployeeGroup
    {
        public string GroupUserId { get; set; }
        public string EmpId { get; set; }
    }
}
