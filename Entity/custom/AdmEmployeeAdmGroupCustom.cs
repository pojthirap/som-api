using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Entity.custom
{

    public class AdmEmployeeAdmGroupCustom
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
        public string createUser { get; set; }
        public DateTime? createDtm { get; set; }
        public string updateUser { get; set; }
        public DateTime? updateDtm { get; set; }
        public string JobTitle { get; set; }

        public decimal? groupId { get; set; }
        public decimal? groupUserId { get; set; }
        public string groupUserType { get; set; }
        public string groupNameTh { get; set; }
        public string groupNameEn { get; set; }
        public DateTime? effectiveDate { get; set; }
        public DateTime? expiryDate { get; set; }


        public string buNameEn { get; set; }
        public string buNameTh { get; set; }
        public decimal? buId { get; set; }
        public string descriptionTh { get; set; }
        public string territoryNameTh { get; set; }




    }
}
