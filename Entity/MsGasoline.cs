using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("MS_GASOLINE")]
    [Index(nameof(GasNameTh), Name = "UK_MS_GASOLINE", IsUnique = true)]
    public partial class MsGasoline
    {
        [Key]
        [Column("GAS_ID", TypeName = "numeric(10, 0)")]
        public decimal GasId { get; set; }
        [Required]
        [Column("GAS_NAME_TH")]
        [StringLength(250)]
        public string GasNameTh { get; set; }
        [Required]
        [Column("GAS_NAME_EN")]
        [StringLength(250)]
        public string GasNameEn { get; set; }
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
        [Column("GAS_CODE")]
        [StringLength(20)]
        public string GasCode { get; set; }
    }
}
