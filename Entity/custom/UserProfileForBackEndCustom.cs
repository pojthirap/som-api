using Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Entity.custom
{

    public class UserProfileForBackEndCustom
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

        public decimal? GroupId { get; set; }
        public string GroupNameTh { get; set; }
        public string GroupNameEn { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpiryDate { get; set; }


        public decimal? GroupUserId { get; set; }
        public string GroupUserType { get; set; }

        public string AdmGroup_GroupCode { get; set; }



        public decimal? BuId { get; set; }
        public string BuCode { get; set; }
        public string BuNameTh { get; set; }
        public string BuNameEn { get; set; }

        //public List<OrgTerritory> OrgTerritory { get; set; }
        public OrgTerritory OrgTerritory { get; set; }
        public List<PermObjCodeCustom> listPermObjCode { get; set; }

    }
}
