using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("TEMPLATE_APP_QUESTION")]
    public partial class TemplateAppQuestion
    {
        [Key]
        [Column("TP_APP_QUESTION_ID", TypeName = "numeric(10, 0)")]
        public decimal TpAppQuestionId { get; set; }
        [Column("TP_APP_FORM_ID", TypeName = "numeric(10, 0)")]
        public decimal? TpAppFormId { get; set; }
        [Column("QUESTION_ID", TypeName = "numeric(10, 0)")]
        public decimal? QuestionId { get; set; }
        [Column("ORDER_NO", TypeName = "numeric(2, 0)")]
        public decimal? OrderNo { get; set; }
        [Column("PREREQUIST_ORDER_NO", TypeName = "numeric(10, 0)")]
        public decimal? PrerequistOrderNo { get; set; }
        [Column("REQUIRE_FLAG")]
        [StringLength(1)]
        public string RequireFlag { get; set; }
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
