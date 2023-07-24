using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("EMAIL_JOB")]
    public partial class EmailJob
    {
        [Key]
        [Column("JOB_ID", TypeName = "numeric(10, 0)")]
        public decimal JobId { get; set; }
        [Column("EMAIL_TEMPLATE_ID", TypeName = "numeric(10, 0)")]
        public decimal EmailTemplateId { get; set; }
        [Required]
        [Column("TABLE_REF_KEY_ID")]
        [StringLength(10)]
        public string TableRefKeyId { get; set; }
        [Required]
        [Column("OBJ_EMAIL", TypeName = "text")]
        public string ObjEmail { get; set; }
        [Required]
        [Column("JOB_STATUS")]
        [StringLength(1)]
        public string JobStatus { get; set; }
        [Column("ERROR_DESC", TypeName = "text")]
        public string ErrorDesc { get; set; }
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
        [Column("JOB_STATUS_FAIL_COUNT", TypeName = "numeric(2, 0)")]
        public decimal? JobStatusFailCount { get; set; }
    }
}
