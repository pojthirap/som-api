using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("SALE_ORDER_CHANGE_LOG")]
    public partial class SaleOrderChangeLog
    {
        [Key]
        [Column("LOG_ID", TypeName = "numeric(10, 0)")]
        public decimal LogId { get; set; }
        [Column("ORDER_ID", TypeName = "numeric(10, 0)")]
        public decimal OrderId { get; set; }
        [Required]
        [Column("CHANGE_TAB_DESC")]
        [StringLength(250)]
        public string ChangeTabDesc { get; set; }
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
        [Column("ORDER_SALE_REP")]
        [StringLength(20)]
        public string OrderSaleRep { get; set; }
    }
}
