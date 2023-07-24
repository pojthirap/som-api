using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Models.saleorder
{
    public class CreateSaleOrderModel
    {

        public SaleOrderCreate SaleOrder { get; set; }
        public List<SaleOrderProduct> items;
        

        public class SaleOrderCreate
        {
            public string OrderId { get; set; }
            public string QuotationNo { get; set; }
            public string CustCode { get; set; }
            public string SomOrderNo { get; set; }
            public string SomOrderDte { get; set; }
            public string SapOrderNo { get; set; }
            public string SimulateDtm { get; set; }
            public string PricingDtm { get; set; }
            public string DocTypeCode { get; set; }
            public string ShipToCustPartnerId { get; set; }
            public string Description { get; set; }
            public string DeliveryDte { get; set; }
            public string SaleRep { get; set; }
            public string GroupCode { get; set; }
            public string OrgCode { get; set; }
            public string ChannelCode { get; set; }
            public string DivisionCode { get; set; }
            public string Territory { get; set; }
            public string ContactPerson { get; set; }
            public string Remark { get; set; }
            public string ReasonCode { get; set; }
            public string ReasonReject { get; set; }
            public string NetValue { get; set; }
            public string Tax { get; set; }
            public string Total { get; set; }
            public string PaymentTerm { get; set; }
            public string Incoterm { get; set; }
            public string PlantCode { get; set; }
            public string ShipCode { get; set; }
            public string SaleSup { get; set; }
            public string OrderStatus { get; set; }
            public string CreditStatus { get; set; }
            public string SapMsg { get; set; }
            public string SapStatus { get; set; }
            public string SapOrderDte { get; set; }
            public string CustSaleId { get; set; }
            public string ShipToCustCode { get; set; }
            public string PoNo { get; set; }
        }
    }
}
