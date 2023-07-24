using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("MS_CATEGORY")]
    public partial class MsCategory
    {
        [Key]
        [Column("CATE_ID", TypeName = "numeric(10, 0)")]
        public decimal CateId { get; set; }
        [Required]
        [Column("CATE_CODE")]
        [StringLength(10)]
        public string CateCode { get; set; }
        [Required]
        [Column("CATE_NAME_TH")]
        [StringLength(250)]
        public string CateNameTh { get; set; }
        [Required]
        [Column("CATE_NAME_EN")]
        [StringLength(250)]
        public string CateNameEn { get; set; }
        [Required]
        [Column("ACTIVE_FLAG")]
        [StringLength(1)]
        public string ActiveFlag { get; set; }
        [Required]
        [Column("CREATE_USER")]
        [StringLength(50)]
        public string CreateUser { get; set; }
        [Column("CREATE_DTM", TypeName = "datetime")]
        public DateTime CreateDtm { get; set; }
        [Required]
        [Column("UPDATE_USER")]
        [StringLength(50)]
        public string UpdateUser { get; set; }
        [Column("UPDATE_DTM", TypeName = "datetime")]
        public DateTime UpdateDtm { get; set; }
    }
}
