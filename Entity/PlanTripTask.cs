using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("PLAN_TRIP_TASK")]
    public partial class PlanTripTask
    {
        [Key]
        [Column("PLAN_TRIP_TASK_ID", TypeName = "numeric(10, 0)")]
        public decimal PlanTripTaskId { get; set; }
        [Column("PLAN_TRIP_PROSP_ID", TypeName = "numeric(10, 0)")]
        public decimal? PlanTripProspId { get; set; }
        [Column("TASK_TYPE")]
        [StringLength(1)]
        public string TaskType { get; set; }
        [Column("TP_STOCK_CARD_ID", TypeName = "numeric(10, 0)")]
        public decimal? TpStockCardId { get; set; }
        [Column("TP_SA_FORM_ID", TypeName = "numeric(10, 0)")]
        public decimal? TpSaFormId { get; set; }
        [Column("TP_APP_FORM_ID", TypeName = "numeric(10, 0)")]
        public decimal? TpAppFormId { get; set; }
        [Column("REQUIRE_FLAG")]
        [StringLength(1)]
        public string RequireFlag { get; set; }
        [Column("ORDER_NO", TypeName = "numeric(2, 0)")]
        public decimal? OrderNo { get; set; }
        [Column("ADHOC_FLAG")]
        [StringLength(1)]
        public string AdhocFlag { get; set; }
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
