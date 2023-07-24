using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("MS_BRAND")]
    [Index(nameof(BrandNameTh), Name = "UK_MS_BRAND", IsUnique = true)]
    public partial class MsBrand
    {
        [Key]
        [Column("BRAND_ID", TypeName = "numeric(10, 0)")]
        public decimal BrandId { get; set; }
        [Required]
        [Column("BRAND_CODE")]
        [StringLength(10)]
        public string BrandCode { get; set; }
        [Required]
        [Column("BRAND_NAME_TH")]
        [StringLength(250)]
        public string BrandNameTh { get; set; }
        [Required]
        [Column("BRAND_NAME_EN")]
        [StringLength(250)]
        public string BrandNameEn { get; set; }
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
