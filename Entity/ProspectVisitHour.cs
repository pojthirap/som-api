using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("PROSPECT_VISIT_HOUR")]
    public partial class ProspectVisitHour
    {
        [Key]
        [Column("PROSP_VISIT_HR_ID", TypeName = "numeric(10, 0)")]
        public decimal ProspVisitHrId { get; set; }
        [Column("PROSPECT_ID", TypeName = "numeric(10, 0)")]
        public decimal? ProspectId { get; set; }
        [Column("DAYS_CODE")]
        [StringLength(1)]
        public string DaysCode { get; set; }
        [Column("HOUR_START")]
        [StringLength(5)]
        public string HourStart { get; set; }
        [Column("HOUR_END")]
        [StringLength(5)]
        public string HourEnd { get; set; }
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
