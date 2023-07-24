using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("MS_COUNTRY")]
    public partial class MsCountry
    {
        [Key]
        [Column("COUNTRY_CODE")]
        [StringLength(10)]
        public string CountryCode { get; set; }
        [Column("LANG_KEY")]
        [StringLength(10)]
        public string LangKey { get; set; }
        [Column("COUNTRY_NAME_TH")]
        [StringLength(250)]
        public string CountryNameTh { get; set; }
        [Column("COUNTRY_NAME_EN")]
        [StringLength(250)]
        public string CountryNameEn { get; set; }
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
