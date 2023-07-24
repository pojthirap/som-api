using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Entity.custom
{
    [Keyless]
    public class OrgSearchRepCustom
    {



        // AdmEmployee
        public string EmpId { get; set; }
        public String CompanyCode { get; set; }
        public String TitleName { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Gender { get; set; }
        public String Street { get; set; }
        public String TellNo { get; set; }
        public String CountryName { get; set; }
        public String ProvinceCode { get; set; }
        public String GroupCode { get; set; }
        public String DistrictName { get; set; }
        public String SubdistrictName { get; set; }
        public String PostCode { get; set; }
        public String Email { get; set; }
        public String Status { get; set; }
        public String ApproveEmpId { get; set; }
        public String JobTitle { get; set; }
        public String ActiveFlag { get; set; }
        public String CreateUser { get; set; }
        public DateTime? CreateDtm { get; set; }
        public String UpdateUser { get; set; }
        public DateTime? UpdateDtm { get; set; }



        // OrgSaleGroup
        public String OfficeCode { get; set; }
        public String DescriptionTh { get; set; }
        public String DescriptionEn { get; set; }
        public String? ManagerEmpId { get; set; }






    }
}
