using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("TEMPLATE_STOCK_CARD")]
    [Index(nameof(TpNameTh), Name = "UK_TEMPLATE_STOCK_CARD", IsUnique = true)]
    public partial class TemplateStockCard
    {
        [Key]
        [Column("TP_STOCK_CARD_ID", TypeName = "numeric(10, 0)")]
        public decimal TpStockCardId { get; set; }
        [Required]
        [Column("TP_CODE")]
        [StringLength(20)]
        public string TpCode { get; set; }
        [Required]
        [Column("TP_NAME_TH")]
        [StringLength(250)]
        public string TpNameTh { get; set; }
        [Required]
        [Column("TP_NAME_EN")]
        [StringLength(250)]
        public string TpNameEn { get; set; }
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
    }
}
