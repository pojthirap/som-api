using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("MS_CHECKPOINT")]
    public partial class MsCheckpoint
    {
        [Key]
        [Column("CHECKPOINT_ID", TypeName = "numeric(10, 0)")]
        public decimal CheckpointId { get; set; }
        [Required]
        [Column("CHECKPOINT_CODE")]
        [StringLength(10)]
        public string CheckpointCode { get; set; }
        [Required]
        [Column("CHECKPOINT_NAME_TH")]
        [StringLength(250)]
        public string CheckpointNameTh { get; set; }
        [Required]
        [Column("CHECKPOINT_NAME_EN")]
        [StringLength(250)]
        public string CheckpointNameEn { get; set; }
        [Required]
        [Column("ADDRESS")]
        [StringLength(50)]
        public string Address { get; set; }
        [Column("SUB_DISTRICT_ID", TypeName = "numeric(10, 0)")]
        public decimal SubDistrictId { get; set; }
        [Column("DISTRICT_ID", TypeName = "numeric(10, 0)")]
        public decimal DistrictId { get; set; }
        [Column("PROVINCE_ID", TypeName = "numeric(10, 0)")]
        public decimal ProvinceId { get; set; }
        [Required]
        [Column("ACTIVE_FLAG")]
        [StringLength(1)]
        public string ActiveFlag { get; set; }
        [Required]
        [Column("CREATE_USER")]
        [StringLength(50)]
        public string CreateUser { get; set; }
        [Column("CREATE_DTM", TypeName = "datetime")]
        public DateTime CreateDtm { get; set; }
        [Required]
        [Column("UPDATE_USER")]
        [StringLength(250)]
        public string UpdateUser { get; set; }
        [Column("UPDATE_DTM", TypeName = "datetime")]
        public DateTime UpdateDtm { get; set; }
    }
}
