using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("PLAN_TRIP")]
    public partial class PlanTrip
    {
        [Key]
        [Column("PLAN_TRIP_ID", TypeName = "numeric(10, 0)")]
        public decimal PlanTripId { get; set; }
        [Column("PLAN_TRIP_NAME")]
        [StringLength(250)]
        public string PlanTripName { get; set; }
        [Column("PLAN_TRIP_DATE", TypeName = "datetime")]
        public DateTime? PlanTripDate { get; set; }
        [Column("ASSIGN_EMP_ID")]
        [StringLength(20)]
        public string AssignEmpId { get; set; }
        [Column("REMARK")]
        [StringLength(250)]
        public string Remark { get; set; }
        [Column("START_CHECKIN_LOC_ID", TypeName = "numeric(10, 0)")]
        public decimal? StartCheckinLocId { get; set; }
        [Column("START_CHECKIN_MILE_NO", TypeName = "numeric(10, 0)")]
        public decimal? StartCheckinMileNo { get; set; }
        [Column("START_CHECKIN_DTM", TypeName = "datetime")]
        public DateTime? StartCheckinDtm { get; set; }
        [Column("START_CHECKOUT_DTM", TypeName = "datetime")]
        public DateTime? StartCheckoutDtm { get; set; }
        [Column("STOP_CHECKIN_LOC_ID", TypeName = "numeric(10, 0)")]
        public decimal? StopCheckinLocId { get; set; }
        [Column("STOP_CHECKIN_MILE_NO", TypeName = "numeric(10, 0)")]
        public decimal? StopCheckinMileNo { get; set; }
        [Column("STOP_CHECKIN_DTM", TypeName = "datetime")]
        public DateTime? StopCheckinDtm { get; set; }
        [Column("STATUS")]
        [StringLength(1)]
        public string Status { get; set; }
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
        [Column("STOP_CALC_KM", TypeName = "decimal(10, 2)")]
        public decimal? StopCalcKm { get; set; }
        [Column("MERG_PLAN_TRIP_ID", TypeName = "numeric(10, 0)")]
        public decimal? MergPlanTripId { get; set; }
    }
}
