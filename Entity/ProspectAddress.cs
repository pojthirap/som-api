using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("PROSPECT_ADDRESS")]
    public partial class ProspectAddress
    {
        [Key]
        [Column("PROSP_ADDR_ID", TypeName = "numeric(10, 0)")]
        public decimal ProspAddrId { get; set; }
        [Column("PROSPECT_ID", TypeName = "numeric(10, 0)")]
        public decimal? ProspectId { get; set; }
        [Column("PROSP_ACC_ID", TypeName = "numeric(10, 0)")]
        public decimal? ProspAccId { get; set; }
        [Column("ADDR_NO")]
        [StringLength(50)]
        public string AddrNo { get; set; }
        [Column("MOO")]
        [StringLength(50)]
        public string Moo { get; set; }
        [Column("SOI")]
        [StringLength(50)]
        public string Soi { get; set; }
        [Column("STREET")]
        [StringLength(50)]
        public string Street { get; set; }
        [Column("TELL_NO")]
        [StringLength(20)]
        public string TellNo { get; set; }
        [Column("FAX_NO")]
        [StringLength(20)]
        public string FaxNo { get; set; }
        [Column("LATITUDE")]
        [StringLength(50)]
        public string Latitude { get; set; }
        [Column("LONGITUDE")]
        [StringLength(50)]
        public string Longitude { get; set; }
        [Column("REGION_CODE")]
        [StringLength(10)]
        public string RegionCode { get; set; }
        [Column("PROVINCE_CODE")]
        [StringLength(10)]
        public string ProvinceCode { get; set; }
        [Column("PROVINCE_DBD")]
        [StringLength(250)]
        public string ProvinceDbd { get; set; }
        [Column("DISTRICT_CODE")]
        [StringLength(10)]
        public string DistrictCode { get; set; }
        [Column("SUBDISTRICT_CODE")]
        [StringLength(10)]
        public string SubdistrictCode { get; set; }
        [Column("POST_CODE")]
        [StringLength(5)]
        public string PostCode { get; set; }
        [Column("REMARK")]
        [StringLength(250)]
        public string Remark { get; set; }
        [Column("MAIN_FLAG")]
        [StringLength(1)]
        public string MainFlag { get; set; }
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
