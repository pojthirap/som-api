using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("RECORD_METER")]
    public partial class RecordMeter
    {
        [Key]
        [Column("REC_METER_ID", TypeName = "numeric(10, 0)")]
        public decimal RecMeterId { get; set; }
        [Column("PLAN_TRIP_TASK_ID", TypeName = "numeric(10, 0)")]
        public decimal? PlanTripTaskId { get; set; }
        [Column("METER_ID", TypeName = "numeric(10, 0)")]
        public decimal? MeterId { get; set; }
        [Column("REC_RUN_NO")]
        [StringLength(7)]
        public string RecRunNo { get; set; }
        [Column("FILE_ID", TypeName = "numeric(10, 0)")]
        public decimal? FileId { get; set; }
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
        [Column("REMARK")]
        [StringLength(250)]
        public string Remark { get; set; }
        [Column("PREV_GAS_ID", TypeName = "numeric(10, 0)")]
        public decimal? PrevGasId { get; set; }
        [Column("GAS_ID", TypeName = "numeric(10, 0)")]
        public decimal? GasId { get; set; }
        [Column("PREV_REC_RUN_NO")]
        [StringLength(7)]
        public string PrevRecRunNo { get; set; }
        [Column("PREV_REC_METER_ID", TypeName = "numeric(10, 0)")]
        public decimal? PrevRecMeterId { get; set; }
    }
}
