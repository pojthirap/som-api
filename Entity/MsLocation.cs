using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("MS_LOCATION")]
    [Index(nameof(LocNameTh), Name = "UK_MS_LOCATION", IsUnique = true)]
    public partial class MsLocation
    {
        [Key]
        [Column("LOC_ID", TypeName = "numeric(10, 0)")]
        public decimal LocId { get; set; }
        [Column("LOC_TYPE_ID", TypeName = "numeric(10, 0)")]
        public decimal LocTypeId { get; set; }
        [Required]
        [Column("LOC_CODE")]
        [StringLength(10)]
        public string LocCode { get; set; }
        [Required]
        [Column("LOC_NAME_TH")]
        [StringLength(250)]
        public string LocNameTh { get; set; }
        [Required]
        [Column("LOC_NAME_EN")]
        [StringLength(250)]
        public string LocNameEn { get; set; }
        [Required]
        [Column("PROVINCE_CODE")]
        [StringLength(10)]
        public string ProvinceCode { get; set; }
        [Required]
        [Column("LATITUDE")]
        [StringLength(50)]
        public string Latitude { get; set; }
        [Required]
        [Column("LONGITUDE")]
        [StringLength(50)]
        public string Longitude { get; set; }
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
