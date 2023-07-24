using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("SALE_ORDER_PRODUCT_LOG")]
    public partial class SaleOrderProductLog
    {
        [Key]
        [Column("ORDER_PROD_LOG_ID", TypeName = "numeric(10, 0)")]
        public decimal OrderProdLogId { get; set; }
        [Column("ORDER_LOG_ID", TypeName = "numeric(10, 0)")]
        public decimal OrderLogId { get; set; }
        [Required]
        [Column("PROD_CODE")]
        [StringLength(20)]
        public string ProdCode { get; set; }
        [Column("QTY", TypeName = "numeric(10, 0)")]
        public decimal Qty { get; set; }
        [Column("PROD_CONV_ID", TypeName = "numeric(10, 0)")]
        public decimal ProdConvId { get; set; }
        [Column("NET_PRICE_EX", TypeName = "decimal(10, 2)")]
        public decimal? NetPriceEx { get; set; }
        [Column("TRANSFER_PRICE", TypeName = "decimal(10, 2)")]
        public decimal? TransferPrice { get; set; }
        [Column("NET_PRICE_INC", TypeName = "decimal(10, 2)")]
        public decimal? NetPriceInc { get; set; }
        [Column("ADDITIONAL_PRICE", TypeName = "decimal(10, 2)")]
        public decimal? AdditionalPrice { get; set; }
        [Column("ITEM_TYPE")]
        [StringLength(50)]
        public string ItemType { get; set; }
        [Column("NET_VALUE", TypeName = "decimal(10, 2)")]
        public decimal? NetValue { get; set; }
        [Required]
        [Column("CREATE_USER")]
        [StringLength(20)]
        public string CreateUser { get; set; }
        [Column("CREATE_DTM", TypeName = "datetime")]
        public DateTime CreateDtm { get; set; }
        [Required]
        [Column("UPDATE_USER")]
        [StringLength(20)]
        public string UpdateUser { get; set; }
        [Column("UPDATE_DTM", TypeName = "datetime")]
        public DateTime UpdateDtm { get; set; }
        [Column("SAP_ITEM_NO")]
        [StringLength(5)]
        public string SapItemNo { get; set; }
        [Column("PROD_ALT_UNIT")]
        [StringLength(5)]
        public string ProdAltUnit { get; set; }
        [Column("PROD_CATE_CODE")]
        [StringLength(20)]
        public string ProdCateCode { get; set; }
        [Column("ADDITIONAL_PER_UNIT", TypeName = "numeric(2, 0)")]
        public decimal? AdditionalPerUnit { get; set; }
    }
}
