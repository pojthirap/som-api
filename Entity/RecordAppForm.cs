using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("RECORD_APP_FORM")]
    public partial class RecordAppForm
    {
        [Key]
        [Column("REC_APP_FORM_ID", TypeName = "numeric(10, 0)")]
        public decimal RecAppFormId { get; set; }
        [Column("PLAN_TRIP_TASK_ID", TypeName = "numeric(10, 0)")]
        public decimal? PlanTripTaskId { get; set; }
        [Required]
        [Column("APP_FORM", TypeName = "text")]
        public string AppForm { get; set; }
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
        [Column("TP_APP_FORM_ID", TypeName = "numeric(10, 0)")]
        public decimal? TpAppFormId { get; set; }
        [Column("PROSP_ID", TypeName = "numeric(10, 0)")]
        public decimal? ProspId { get; set; }
    }
}
