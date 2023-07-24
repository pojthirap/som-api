using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("CUSTOMER_SALE")]
    public partial class CustomerSale
    {
        [Key]
        [Column("CUST_SALE_ID", TypeName = "numeric(10, 0)")]
        public decimal CustSaleId { get; set; }
        [Column("CUST_CODE")]
        [StringLength(20)]
        public string CustCode { get; set; }
        [Column("OFFICE_AREA_ID", TypeName = "numeric(10, 0)")]
        public decimal? OfficeAreaId { get; set; }
        [Column("CUST_GROUP")]
        [StringLength(50)]
        public string CustGroup { get; set; }
        [Column("PAYMENT_TERM")]
        [StringLength(20)]
        public string PaymentTerm { get; set; }
        [Column("INCOTERM")]
        [StringLength(20)]
        public string Incoterm { get; set; }
        [Column("ACTIVE_FLAG")]
        [StringLength(1)]
        public string ActiveFlag { get; set; }
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
        [Column("ORG_CODE")]
        [StringLength(10)]
        public string OrgCode { get; set; }
        [Column("CHANNEL_CODE")]
        [StringLength(10)]
        public string ChannelCode { get; set; }
        [Column("DIVISION_CODE")]
        [StringLength(10)]
        public string DivisionCode { get; set; }
        [Column("GROUP_CODE")]
        [StringLength(10)]
        public string GroupCode { get; set; }
        [Column("OFFICE_CODE")]
        [StringLength(10)]
        public string OfficeCode { get; set; }
        [Column("SYNC_ID", TypeName = "numeric(10, 0)")]
        public decimal? SyncId { get; set; }
        [Column("SHIPPING_COND")]
        [StringLength(5)]
        public string ShippingCond { get; set; }
    }
}
