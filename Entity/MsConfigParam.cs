using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("MS_CONFIG_PARAM")]
    public partial class MsConfigParam
    {
        [Key]
        [Column("PARAM_KEYWORD")]
        [StringLength(50)]
        public string ParamKeyword { get; set; }
        [Column("PARAM_NAME_TH")]
        [StringLength(250)]
        public string ParamNameTh { get; set; }
        [Column("PARAM_NAME_EN")]
        [StringLength(250)]
        public string ParamNameEn { get; set; }
        [Column("PARAM_DESC")]
        [StringLength(250)]
        public string ParamDesc { get; set; }
        [Column("PARAM_VALUE")]
        [StringLength(250)]
        public string ParamValue { get; set; }
        [Column("SYSTEM_FLAG")]
        [StringLength(1)]
        public string SystemFlag { get; set; }
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
