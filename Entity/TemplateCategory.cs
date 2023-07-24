using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("TEMPLATE_CATEGORY")]
    [Index(nameof(TpCateNameTh), Name = "UK_TEMPLATE_CATEGORY", IsUnique = true)]
    public partial class TemplateCategory
    {
        [Key]
        [Column("TP_CATE_ID", TypeName = "numeric(10, 0)")]
        public decimal TpCateId { get; set; }
        [Required]
        [Column("TP_CATE_CODE")]
        [StringLength(10)]
        public string TpCateCode { get; set; }
        [Required]
        [Column("TP_CATE_NAME_TH")]
        [StringLength(250)]
        public string TpCateNameTh { get; set; }
        [Required]
        [Column("TP_CATE_NAME_EN")]
        [StringLength(250)]
        public string TpCateNameEn { get; set; }
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
