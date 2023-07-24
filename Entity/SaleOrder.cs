using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("SALE_ORDER")]
    public partial class SaleOrder
    {
        [Key]
        [Column("ORDER_ID", TypeName = "numeric(10, 0)")]
        public decimal OrderId { get; set; }
        [Column("QUOTATION_NO")]
        [StringLength(20)]
        public string QuotationNo { get; set; }
        [Column("CUST_CODE")]
        [StringLength(20)]
        public string CustCode { get; set; }
        [Column("SOM_ORDER_NO")]
        [StringLength(20)]
        public string SomOrderNo { get; set; }
        [Column("SOM_ORDER_DTE", TypeName = "datetime")]
        public DateTime? SomOrderDte { get; set; }
        [Column("SAP_ORDER_NO")]
        [StringLength(20)]
        public string SapOrderNo { get; set; }
        [Column("SIMULATE_DTM", TypeName = "datetime")]
        public DateTime? SimulateDtm { get; set; }
        [Column("PRICING_DTM", TypeName = "datetime")]
        public DateTime? PricingDtm { get; set; }
        [Column("DOC_TYPE_CODE")]
        [StringLength(10)]
        public string DocTypeCode { get; set; }
        [Column("SHIP_TO_CUST_PARTNER_ID", TypeName = "numeric(10, 0)")]
        public decimal? ShipToCustPartnerId { get; set; }
        [Column("DESCRIPTION")]
        [StringLength(250)]
        public string Description { get; set; }
        [Column("DELIVERY_DTE", TypeName = "datetime")]
        public DateTime? DeliveryDte { get; set; }
        [Column("SALE_REP")]
        [StringLength(20)]
        public string SaleRep { get; set; }
        [Column("GROUP_CODE")]
        [StringLength(10)]
        public string GroupCode { get; set; }
        [Column("ORG_CODE")]
        [StringLength(10)]
        public string OrgCode { get; set; }
        [Column("CHANNEL_CODE")]
        [StringLength(10)]
        public string ChannelCode { get; set; }
        [Column("DIVISION_CODE")]
        [StringLength(10)]
        public string DivisionCode { get; set; }
        [Column("TERRITORY")]
        [StringLength(250)]
        public string Territory { get; set; }
        [Column("CONTACT_PERSON")]
        [StringLength(250)]
        public string ContactPerson { get; set; }
        [Column("REMARK")]
        [StringLength(250)]
        public string Remark { get; set; }
        [Column("REASON_CODE")]
        [StringLength(10)]
        public string ReasonCode { get; set; }
        [Column("REASON_REJECT")]
        [StringLength(250)]
        public string ReasonReject { get; set; }
        [Column("NET_VALUE", TypeName = "decimal(20, 2)")]
        public decimal? NetValue { get; set; }
        [Column("TAX", TypeName = "decimal(20, 2)")]
        public decimal? Tax { get; set; }
        [Column("TOTAL", TypeName = "decimal(20, 2)")]
        public decimal? Total { get; set; }
        [Column("PAYMENT_TERM")]
        [StringLength(10)]
        public string PaymentTerm { get; set; }
        [Column("INCOTERM")]
        [StringLength(10)]
        public string Incoterm { get; set; }
        [Column("PLANT_CODE")]
        [StringLength(10)]
        public string PlantCode { get; set; }
        [Column("SHIP_CODE")]
        [StringLength(10)]
        public string ShipCode { get; set; }
        [Column("SALE_SUP")]
        [StringLength(20)]
        public string SaleSup { get; set; }
        [Column("ORDER_STATUS")]
        [StringLength(1)]
        public string OrderStatus { get; set; }
        [Column("CREDIT_STATUS")]
        [StringLength(100)]
        public string CreditStatus { get; set; }
        [Column("SAP_MSG")]
        [StringLength(250)]
        public string SapMsg { get; set; }
        [Column("CREATE_USER")]
        [StringLength(20)]
        public string CreateUser { get; set; }
        [Column("CREATE_DTM", TypeName = "datetime")]
        public DateTime? CreateDtm { get; set; }
        [Column("UPDATE_USER")]
        [StringLength(20)]
        public string UpdateUser { get; set; }
        [Column("UPDATE_DTM", TypeName = "datetime")]
        public DateTime? UpdateDtm { get; set; }
        [Column("SAP_STATUS")]
        [StringLength(250)]
        public string SapStatus { get; set; }
        [Column("SAP_ORDER_DTE", TypeName = "date")]
        public DateTime? SapOrderDte { get; set; }
        [Column("CUST_SALE_ID", TypeName = "numeric(10, 0)")]
        public decimal? CustSaleId { get; set; }
        [Column("SHIP_TO_CUST_CODE")]
        [StringLength(20)]
        public string ShipToCustCode { get; set; }
        [Column("COMPANY_CODE")]
        [StringLength(20)]
        public string CompanyCode { get; set; }
        [Column("ORDER_ACTION")]
        [StringLength(1)]
        public string OrderAction { get; set; }
        [Column("PRICE_LIST")]
        [StringLength(5)]
        public string PriceList { get; set; }
        [Column("PO_NO")]
        [StringLength(20)]
        public string PoNo { get; set; }
    }
}
