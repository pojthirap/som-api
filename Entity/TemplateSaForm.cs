using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("TEMPLATE_SA_FORM")]
    [Index(nameof(TpNameTh), Name = "UK_TEMPLATE_SA_FORM", IsUnique = true)]
    public partial class TemplateSaForm
    {
        [Key]
        [Column("TP_SA_FORM_ID", TypeName = "numeric(10, 0)")]
        public decimal TpSaFormId { get; set; }
        [Column("TP_CODE")]
        [StringLength(20)]
        public string TpCode { get; set; }
        [Column("TP_NAME_TH")]
        [StringLength(250)]
        public string TpNameTh { get; set; }
        [Column("TP_NAME_EN")]
        [StringLength(250)]
        public string TpNameEn { get; set; }
        [Column("USED_FLAG")]
        [StringLength(1)]
        public string UsedFlag { get; set; }
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
        [Column("USED_DTM", TypeName = "datetime")]
        public DateTime? UsedDtm { get; set; }
    }
}
