using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("TEMPLATE_SA_TITLE")]
    [Index(nameof(TpSaFormId), nameof(TitleNameTh), Name = "UK_TEMPLATE_SA_TITLE", IsUnique = true)]
    public partial class TemplateSaTitle
    {
        [Key]
        [Column("TP_SA_TITLE_ID", TypeName = "numeric(10, 0)")]
        public decimal TpSaTitleId { get; set; }
        [Column("TP_SA_FORM_ID", TypeName = "numeric(10, 0)")]
        public decimal? TpSaFormId { get; set; }
        [Column("TITLE_COLM_NO", TypeName = "numeric(2, 0)")]
        public decimal? TitleColmNo { get; set; }
        [Column("TITLE_NAME_TH")]
        [StringLength(250)]
        public string TitleNameTh { get; set; }
        [Column("TITLE_NAME_EN")]
        [StringLength(250)]
        public string TitleNameEn { get; set; }
        [Column("ANS_TYPE")]
        [StringLength(1)]
        public string AnsType { get; set; }
        [Column("ANS_VAL_TYPE", TypeName = "numeric(2, 0)")]
        public decimal? AnsValType { get; set; }
        [Column("ANS_LOV_TYPE", TypeName = "numeric(2, 0)")]
        public decimal? AnsLovType { get; set; }
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
