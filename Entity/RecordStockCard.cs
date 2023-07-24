using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("RECORD_STOCK_CARD")]
    public partial class RecordStockCard
    {
        [Key]
        [Column("REC_STOCK_ID", TypeName = "numeric(10, 0)")]
        public decimal RecStockId { get; set; }
        [Column("PLAN_TRIP_TASK_ID", TypeName = "numeric(10, 0)")]
        public decimal? PlanTripTaskId { get; set; }
        [Column("PROD_CONV_ID", TypeName = "numeric(10, 0)")]
        public decimal? ProdConvId { get; set; }
        [Column("REC_QTY", TypeName = "numeric(5, 0)")]
        public decimal? RecQty { get; set; }
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
