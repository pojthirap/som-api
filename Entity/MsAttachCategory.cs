using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("MS_ATTACH_CATEGORY")]
    [Index(nameof(AttachCateNameTh), Name = "UK_MS_ATTACH_CATEGORY", IsUnique = true)]
    public partial class MsAttachCategory
    {
        [Key]
        [Column("ATTACH_CATE_ID", TypeName = "numeric(10, 0)")]
        public decimal AttachCateId { get; set; }
        [Required]
        [Column("ATTACH_CATE_CODE")]
        [StringLength(10)]
        public string AttachCateCode { get; set; }
        [Required]
        [Column("ATTACH_CATE_NAME_TH")]
        [StringLength(250)]
        public string AttachCateNameTh { get; set; }
        [Column("ATTACH_CATE_NAME_EN")]
        [StringLength(250)]
        public string AttachCateNameEn { get; set; }
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
