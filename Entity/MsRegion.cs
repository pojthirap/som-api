using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("MS_REGION")]
    [Index(nameof(RegionNameTh), Name = "UK_MS_REGION", IsUnique = true)]
    public partial class MsRegion
    {
        [Key]
        [Column("REGION_CODE")]
        [StringLength(10)]
        public string RegionCode { get; set; }
        [Column("REGION_NAME_TH")]
        [StringLength(250)]
        public string RegionNameTh { get; set; }
        [Column("REGION_NAME_EN")]
        [StringLength(250)]
        public string RegionNameEn { get; set; }
        [Column("ACTIVE_FLAG")]
        [StringLength(1)]
        public string ActiveFlag { get; set; }
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
