using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Models.adm
{
    public class AdmEmployeeModel : ModelBase
    {

        public string empId { get; set; }
        public string companyCode { get; set; }
        public string titleName { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string gender { get; set; }
        public string street { get; set; }
        public string tellNo { get; set; }
        public string countryName { get; set; }
        public string provinceCode { get; set; }
        public string groupCode { get; set; }
        public string districtName { get; set; }
        public string subdistrictName { get; set; }
        public string postCode { get; set; }
        public string email { get; set; }
        public string status { get; set; }
        public string approveEmpId { get; set; }
        public string activeFlag { get; set; }
        public string[] empIdList { get; set; }
    }
}
