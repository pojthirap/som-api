using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("SYNC_DATA_LOG")]
    public partial class SyncDataLog
    {
        [Key]
        [Column("SYNC_ID", TypeName = "numeric(10, 0)")]
        public decimal SyncId { get; set; }
        [Column("INTERFACE_ID")]
        [StringLength(10)]
        public string InterfaceId { get; set; }
        [Column("ERROR_DESC", TypeName = "text")]
        public string ErrorDesc { get; set; }
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
