using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("ADM_GROUP_PERM")]
    public partial class AdmGroupPerm
    {
        [Key]
        [Column("GROUP_PERM_ID", TypeName = "numeric(10, 0)")]
        public decimal GroupPermId { get; set; }
        [Column("GROUP_APP_ID", TypeName = "numeric(10, 0)")]
        public decimal? GroupAppId { get; set; }
        [Column("PERM_OBJ_ID", TypeName = "numeric(10, 0)")]
        public decimal? PermObjId { get; set; }
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
