using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Entity.custom
{
    [Keyless]
    public class SearchAdmEmpCustom
    {
        public string EmpId { get; set; }
        public string CompanyCode { get; set; }
        public string JobTitle { get; set; }
        public string TitleName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Street { get; set; }
        public string TellNo { get; set; }
        public string CountryName { get; set; }
        public string ProvinceCode { get; set; }
        public string GroupCode { get; set; }
        public string DistrictName { get; set; }
        public string SubdistrictName { get; set; }
        public string PostCode { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public string ApproveEmpId { get; set; }
        public string ActiveFlag { get; set; }
        public string CreateUser { get; set; }
        public DateTime? CreateDtm { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDtm { get; set; }

        public string ApproverId { get; set; }
        public string ApproveName { get; set; }
        public string BuId { get; set; }

    }
}
