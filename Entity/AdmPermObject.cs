using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("ADM_PERM_OBJECT")]
    public partial class AdmPermObject
    {
        [Key]
        [Column("PERM_OBJ_ID", TypeName = "numeric(10, 0)")]
        public decimal PermObjId { get; set; }
        [Required]
        [Column("PERM_OBJ_CODE")]
        [StringLength(30)]
        public string PermObjCode { get; set; }
        [Required]
        [Column("PERM_OBJ_NAME_TH")]
        [StringLength(250)]
        public string PermObjNameTh { get; set; }
        [Column("PERM_OBJ_NAME_EN")]
        [StringLength(250)]
        public string PermObjNameEn { get; set; }
        [Column("PARENT_ID", TypeName = "numeric(10, 0)")]
        public decimal? ParentId { get; set; }
        [Column("PERM_TYPE")]
        [StringLength(3)]
        public string PermType { get; set; }
        [Column("ORDER_SEQ", TypeName = "numeric(5, 0)")]
        public decimal? OrderSeq { get; set; }
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
