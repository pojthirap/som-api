using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("PLAN_REASON_NOT_VISIT")]
    [Index(nameof(ReasonNameTh), Name = "UK_PLAN_REASON_NOT_VISIT", IsUnique = true)]
    public partial class PlanReasonNotVisit
    {
        [Key]
        [Column("REASON_NOT_VISIT_ID", TypeName = "numeric(10, 0)")]
        public decimal ReasonNotVisitId { get; set; }
        [Required]
        [Column("REASON_CODE")]
        [StringLength(10)]
        public string ReasonCode { get; set; }
        [Required]
        [Column("REASON_NAME_TH")]
        [StringLength(250)]
        public string ReasonNameTh { get; set; }
        [Required]
        [Column("REASON_NAME_EN")]
        [StringLength(250)]
        public string ReasonNameEn { get; set; }
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
