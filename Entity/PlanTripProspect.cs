using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("PLAN_TRIP_PROSPECT")]
    public partial class PlanTripProspect
    {
        [Key]
        [Column("PLAN_TRIP_PROSP_ID", TypeName = "numeric(10, 0)")]
        public decimal PlanTripProspId { get; set; }
        [Column("PLAN_TRIP_ID", TypeName = "numeric(10, 0)")]
        public decimal? PlanTripId { get; set; }
        [Column("PROSP_ID", TypeName = "numeric(10, 0)")]
        public decimal? ProspId { get; set; }
        [Column("PLAN_START_TIME", TypeName = "datetime")]
        public DateTime? PlanStartTime { get; set; }
        [Column("PLAN_END_TIME", TypeName = "datetime")]
        public DateTime? PlanEndTime { get; set; }
        [Column("ORDER_NO", TypeName = "numeric(2, 0)")]
        public decimal? OrderNo { get; set; }
        [Column("ADHOC_FLAG")]
        [StringLength(1)]
        public string AdhocFlag { get; set; }
        [Column("MERG_FLAG")]
        [StringLength(1)]
        public string MergFlag { get; set; }
        [Column("VISIT_LATITUDE")]
        [StringLength(50)]
        public string VisitLatitude { get; set; }
        [Column("VISIT_LONGITUDE")]
        [StringLength(50)]
        public string VisitLongitude { get; set; }
        [Column("VISIT_CHECKIN_MILE_NO", TypeName = "numeric(10, 0)")]
        public decimal? VisitCheckinMileNo { get; set; }
        [Column("VISIT_CHECKIN_DTM", TypeName = "datetime")]
        public DateTime? VisitCheckinDtm { get; set; }
        [Column("VISIT_CHECKOUT_DTM", TypeName = "datetime")]
        public DateTime? VisitCheckoutDtm { get; set; }
        [Column("REMIND")]
        [StringLength(250)]
        public string Remind { get; set; }
        [Column("REASON_NOT_VISIT_ID", TypeName = "numeric(10, 0)")]
        public decimal? ReasonNotVisitId { get; set; }
        [Column("REASON_NOT_VISIT_REMARK")]
        [StringLength(250)]
        public string ReasonNotVisitRemark { get; set; }
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
        [Column("VISIT_CALC_KM", TypeName = "decimal(10, 3)")]
        public decimal? VisitCalcKm { get; set; }
        [Column("MERG_PLAN_TRIP_PROSP_ID", TypeName = "numeric(10, 0)")]
        public decimal? MergPlanTripProspId { get; set; }
        [Column("LOC_ID", TypeName = "numeric(10, 0)")]
        public decimal? LocId { get; set; }
        [Column("LOC_REMARK")]
        [StringLength(250)]
        public string LocRemark { get; set; }
    }
}
