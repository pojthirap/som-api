using Entity;
using MyFirstAzureWebApp.Entity.custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.SearchCriteria
{
    public class GetTaskStockCardForRecordCustom
    {
       
        public class Product : MsProduct
        {

            /*public string ProdCode { get; set; }
            public string DivisionCode { get; set; }
            public string ProdNameTh { get; set; }
            public string ProdNameEn { get; set; }
            public string ProdType { get; set; }
            public string ProdGroup { get; set; }
            public string IndustrySector { get; set; }
            public string OldProdNo { get; set; }
            public string BaseUnit { get; set; }
            public string ActiveFlag { get; set; }
            public string CreateUser { get; set; }
            public DateTime? CreateDtm { get; set; }
            public string UpdateUser { get; set; }
            public DateTime? UpdateDtm { get; set; }
            public decimal? ReportProdConvId { get; set; }*/
            public List<ProductConversion> listProductConversion { get; set; }
        }
        public class ProductConversion
        {
            public MsProductConversion MsProductConversion { get; set; }
            public RecordStockCard RecordStockCard { get; set; }
        }


        // Temp Class


        public class objectResult : MsProduct
        {
            /*public string ProdCode { get; set; }
            public string DivisionCode { get; set; }
            public string ProdNameTh { get; set; }
            public string ProdNameEn { get; set; }
            public string ProdType { get; set; }
            public string ProdGroup { get; set; }
            public string IndustrySector { get; set; }
            public string OldProdNo { get; set; }
            public string BaseUnit { get; set; }
            public string ActiveFlag { get; set; }
            public string CreateUser { get; set; }
            public DateTime? CreateDtm { get; set; }
            public string UpdateUser { get; set; }
            public DateTime? UpdateDtm { get; set; }
            public decimal? ReportProdConvId { get; set; }*/

            public MsProductConversion MsProductConversion { get; set; }
            public RecordStockCard RecordStockCard { get; set; }

        }

        public  class MsProductConversion
        {
            public string? ProdConvId { get; set; }
            public string? ProdCode { get; set; }
            public string? AltUnit { get; set; }
            public string? ActiveFlag { get; set; }
            public string? CreateUser { get; set; }
            public DateTime? CreateDtm { get; set; }
            public string? UpdateUser { get; set; }
            public DateTime? UpdateDtm { get; set; }
            public string? Denominator { get; set; }
            public string? Counter { get; set; }
            public string? GrossWeight { get; set; }
            public string? WeightUnit { get; set; }
        }


        public class RecordStockCard
        {
            public string? RecStockId { get; set; }
            public string? PlanTripTaskId { get; set; }
            public string? ProdConvId { get; set; }
            public string? RecQty { get; set; }
            public string? CreateUser { get; set; }
            public DateTime? CreateDtm { get; set; }
            public string? UpdateUser { get; set; }
            public DateTime? UpdateDtm { get; set; }
        }


    }
}
