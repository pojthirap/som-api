using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("MS_CONFIG_LOV")]
    public partial class MsConfigLov
    {
        //[Key]
        [Column("LOV_KEYWORD")]
        [StringLength(50)]
        public string LovKeyword { get; set; }
        [Key]
        [Column("LOV_KEYVALUE", TypeName = "numeric(2, 0)")]
        public decimal LovKeyvalue { get; set; }
        [Column("LOV_NAME_TH")]
        [StringLength(250)]
        public string LovNameTh { get; set; }
        [Column("LOV_NAME_EN")]
        [StringLength(250)]
        public string LovNameEn { get; set; }
        [Column("LOV_CODE_TH")]
        [StringLength(250)]
        public string LovCodeTh { get; set; }
        [Column("LOV_CODE_EN")]
        [StringLength(250)]
        public string LovCodeEn { get; set; }
        [Column("LOV_ORDER", TypeName = "numeric(2, 0)")]
        public decimal? LovOrder { get; set; }
        [Column("LOV_REMARK")]
        [StringLength(250)]
        public string LovRemark { get; set; }
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
        [Column("CONDITION1")]
        [StringLength(250)]
        public string Condition1 { get; set; }
    }
}
