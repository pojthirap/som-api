using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Entity.custom
{

    public class SaleGroupSaleOfficeForBackEndCustom
    {


        public string GroupCode { get; set; }
        public string OfficeCode { get; set; }
        public string DescriptionTh { get; set; }
        public string DescriptionEn { get; set; }
        public string? ManagerEmpId { get; set; }
        public string ActiveFlag { get; set; }
        public string CreateUser { get; set; }
        public DateTime? CreateDtm { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDtm { get; set; }



        public string SaleOfficeDescriptionTh { get; set; }
        public string SaleOfficeDescriptionEn { get; set; }


        public decimal TerritoryId { get; set; }
        public string TerritoryCode { get; set; }
        public string TerritoryNameTh { get; set; }
        public string TerritoryNameEn { get; set; }
        public string TerritoryManagerEmpId { get; set; }
        public string TerritoryActiveFlag { get; set; }
        public decimal? BuId { get; set; }







    }
}
