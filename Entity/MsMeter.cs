using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("MS_METER")]
    [Index(nameof(Qrcode), Name = "UK_MS_METER_QRCODE", IsUnique = true)]
    public partial class MsMeter
    {
        [Key]
        [Column("METER_ID", TypeName = "numeric(10, 0)")]
        public decimal MeterId { get; set; }
        [Column("GAS_ID", TypeName = "numeric(10, 0)")]
        public decimal GasId { get; set; }
        [Required]
        [Column("CUST_CODE")]
        [StringLength(20)]
        public string CustCode { get; set; }
        [Column("DISPENSER_NO", TypeName = "numeric(2, 0)")]
        public decimal? DispenserNo { get; set; }
        [Column("NOZZLE_NO", TypeName = "numeric(2, 0)")]
        public decimal? NozzleNo { get; set; }
        [Required]
        [Column("QRCODE")]
        [StringLength(20)]
        public string Qrcode { get; set; }
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
