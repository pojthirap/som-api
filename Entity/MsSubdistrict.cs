using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("MS_SUBDISTRICT")]
    public partial class MsSubdistrict
    {
        [Key]
        [Column("SUBDISTRICT_CODE")]
        [StringLength(10)]
        public string SubdistrictCode { get; set; }
        [Column("DISTRICT_CODE")]
        [StringLength(10)]
        public string DistrictCode { get; set; }
        [Required]
        [Column("SUBDISTRICT_NAME_TH")]
        [StringLength(250)]
        public string SubdistrictNameTh { get; set; }
        [Required]
        [Column("SUBDISTRICT_NAME_EN")]
        [StringLength(250)]
        public string SubdistrictNameEn { get; set; }
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
        [Column("SYNC_ID", TypeName = "numeric(10, 0)")]
        public decimal? SyncId { get; set; }
    }
}
