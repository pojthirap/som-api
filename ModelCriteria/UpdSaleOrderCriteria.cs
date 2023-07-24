using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.ModelCriteria
{
    public class UpdSaleOrderCriteria
    {
        public string TypeSubmit { get; set; }
        public string ChangeTabDesc { get; set; }
        public SaleOrderForUpdateSaleOrder SaleOrder { get; set; }
        public List<SaleOrderProductForUpdateSaleOrder> Items { get; set; }
    }

    public class SaleOrderForUpdateSaleOrder 
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
        public string PriceList { get; set; }
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
        public string CompanyCode { get; set; }

        public string PriceDate { get; set; }
        public string PriceTime { get; set; }
        public string PoNo { get; set; }

    }

    public class SaleOrderProductForUpdateSaleOrder
    {
        public string OrderProdId { get; set; }
        public string OrderId { get; set; }
        public string ProdCode { get; set; }
        public string Qty { get; set; }
        public string ProdConvId { get; set; }
        public string NetPriceEx { get; set; }
        public string TransferPrice { get; set; }
        public string NetPriceInc { get; set; }
        public string AdditionalPrice { get; set; }
        public string AdditionalPerUnit { get; set; }
        public string ItemType { get; set; }
        public string NetValue { get; set; }
        public string SapItemNo { get; set; }
        public string ProdAltUnit { get; set; }

        public string PlantCode { get; set; }
        public string ShipCode { get; set; }
        public string ProdCateCode { get; set; }
    }
}
