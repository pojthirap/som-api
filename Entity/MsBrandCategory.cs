using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("MS_BRAND_CATEGORY")]
    [Index(nameof(BrandCateNameTh), Name = "UK_MS_BRAND_CATEGORY", IsUnique = true)]
    public partial class MsBrandCategory
    {
        [Key]
        [Column("BRAND_CATE_ID", TypeName = "numeric(10, 0)")]
        public decimal BrandCateId { get; set; }
        [Required]
        [Column("BRAND_CATE_CODE")]
        [StringLength(10)]
        public string BrandCateCode { get; set; }
        [Required]
        [Column("BRAND_CATE_NAME_TH")]
        [StringLength(250)]
        public string BrandCateNameTh { get; set; }
        [Required]
        [Column("BRAND_CATE_NAME_EN")]
        [StringLength(250)]
        public string BrandCateNameEn { get; set; }
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
