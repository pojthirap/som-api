using Entity;
using MyFirstAzureWebApp.Entity.custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.SearchCriteria
{
    public class SearchProductCustomCustom
    {

        public string ProdCode { get; set; }
        public string DivisionCode { get; set; }
        public string ProdNameTh { get; set; }
        public string ProdNameEn { get; set; }
        public string ProdType { get; set; }
        public string ProdGroup { get; set; }
        public string IndustrySector { get; set; }
        public string OldProdNo { get; set; }
        public string BaseUnit { get; set; }
        public string ProductActiveFlag { get; set; }
        public string CreateUser { get; set; }
        public DateTime? CreateDtm { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDtm { get; set; }
        public decimal? ReportProdConvId { get; set; }


        //public string DivisionCode { get; set; }
        public string DivisionNameTh { get; set; }
        public string DivisionNameEn { get; set; }
        public string DivisionActiveFlag { get; set; }


        public decimal? ProdConvId { get; set; }
        //public string ProdCode { get; set; }
        public string AltUnit { get; set; }
        public string ProductConversionActiveFlag { get; set; }
        public decimal? Denominator { get; set; }
        public decimal? Counter { get; set; }
        public decimal? GrossWeight { get; set; }
        public string WeightUnit { get; set; }



    }
}
