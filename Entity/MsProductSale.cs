using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("MS_PRODUCT_SALE")]
    public partial class MsProductSale
    {
        [Key]
        [Column("PROD_SELL_ID", TypeName = "numeric(10, 0)")]
        public decimal ProdSellId { get; set; }
        [Required]
        [Column("PROD_CODE")]
        [StringLength(20)]
        public string ProdCode { get; set; }
        [Required]
        [Column("ORG_CODE")]
        [StringLength(10)]
        public string OrgCode { get; set; }
        [Required]
        [Column("CHANNEL_CODE")]
        [StringLength(10)]
        public string ChannelCode { get; set; }
        [Required]
        [Column("ACTIVE_FLAG")]
        [StringLength(1)]
        public string ActiveFlag { get; set; }
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
        [Column("SYNC_ID", TypeName = "numeric(10, 0)")]
        public decimal? SyncId { get; set; }
        [Column("PROD_CATE_CODE")]
        [StringLength(20)]
        public string ProdCateCode { get; set; }
        [Column("PROD_CATE_DESC")]
        [StringLength(250)]
        public string ProdCateDesc { get; set; }
    }
}
