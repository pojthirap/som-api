using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("TEMPLATE_STOCK_PRODUCT")]
    [Index(nameof(TpStockCardId), nameof(ProdCode), Name = "UK_TEMPLATE_STOCK_PRODUCT", IsUnique = true)]
    public partial class TemplateStockProduct
    {
        [Key]
        [Column("TP_STOCK_PROD_ID", TypeName = "numeric(10, 0)")]
        public decimal TpStockProdId { get; set; }
        [Column("TP_STOCK_CARD_ID", TypeName = "numeric(10, 0)")]
        public decimal? TpStockCardId { get; set; }
        [Column("PROD_CODE")]
        [StringLength(20)]
        public string ProdCode { get; set; }
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
    }
}
