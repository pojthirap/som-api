using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("MS_PROVINCE")]
    public partial class MsProvince
    {
        [Key]
        [Column("PROVINCE_CODE")]
        [StringLength(10)]
        public string ProvinceCode { get; set; }
        [Column("COUNTRY_CODE")]
        [StringLength(10)]
        public string CountryCode { get; set; }
        [Column("REGION_CODE")]
        [StringLength(10)]
        public string RegionCode { get; set; }
        [Column("PROVINCE_NAME_TH")]
        [StringLength(250)]
        public string ProvinceNameTh { get; set; }
        [Column("PROVINCE_NAME_EN")]
        [StringLength(250)]
        public string ProvinceNameEn { get; set; }
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
        [Column("SYNC_ID", TypeName = "numeric(10, 0)")]
        public decimal? SyncId { get; set; }
    }
}
