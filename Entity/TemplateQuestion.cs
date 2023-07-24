using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("TEMPLATE_QUESTION")]
    [Index(nameof(QuestionNameEn), Name = "UK_TEMPLATE_QUESTION", IsUnique = true)]
    public partial class TemplateQuestion
    {
        [Key]
        [Column("QUESTION_ID", TypeName = "numeric(10, 0)")]
        public decimal QuestionId { get; set; }
        [Column("QUESTION_CODE")]
        [StringLength(10)]
        public string QuestionCode { get; set; }
        [Required]
        [Column("QUESTION_NAME_TH")]
        [StringLength(250)]
        public string QuestionNameTh { get; set; }
        [Required]
        [Column("QUESTION_NAME_EN")]
        [StringLength(250)]
        public string QuestionNameEn { get; set; }
        [Column("ANS_TYPE", TypeName = "numeric(2, 0)")]
        public decimal AnsType { get; set; }
        [Column("ANS_VALUES", TypeName = "text")]
        public string AnsValues { get; set; }
        [Column("PUBLIC_FLAG")]
        [StringLength(1)]
        public string PublicFlag { get; set; }
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
