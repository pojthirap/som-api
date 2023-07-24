using Entity;
using MyFirstAzureWebApp.Entity.custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.SearchCriteria
{
    public class SearchTemplateByStockCardCustom
    {
        public decimal TpStockProdId { get; set; }
        public decimal? TpStockCardId { get; set; }
        public string ProdCode { get; set; }
        public string ActiveFlag { get; set; }
        public string CreateUser { get; set; }
        public DateTime? CreateDtm { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDtm { get; set; }


        //public string ProdCode { get; set; }
        public string DivisionCode { get; set; }
        public string ProdNameTh { get; set; }
        public string ProdNameEn { get; set; }
        public string ProdType { get; set; }
        public string ProdGroup { get; set; }
        public string IndustrySector { get; set; }
        public string OldProdNo { get; set; }
        public string BaseUnit { get; set; }
        //public string ActiveFlag { get; set; }


        //public string DivisionCode { get; set; }
        public string DivisionNameTh { get; set; }
        public string DivisionNameEn { get; set; }
        //public string ActiveFlag { get; set; }
    }
}
