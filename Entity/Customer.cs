using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("CUSTOMER")]
    public partial class Customer
    {
        [Key]
        [Column("CUST_CODE")]
        [StringLength(20)]
        public string CustCode { get; set; }
        [Column("PROSPECT_ID", TypeName = "numeric(10, 0)")]
        public decimal? ProspectId { get; set; }
        [Column("ACC_GROUP_CODE")]
        [StringLength(20)]
        public string AccGroupCode { get; set; }
        [Column("CUST_NAME_TH")]
        [StringLength(250)]
        public string CustNameTh { get; set; }
        [Column("CUST_NAME_EN")]
        [StringLength(250)]
        public string CustNameEn { get; set; }
        [Column("SEARCH_TERM")]
        [StringLength(250)]
        public string SearchTerm { get; set; }
        [Column("TRANSPORT_ZONE")]
        [StringLength(20)]
        public string TransportZone { get; set; }
        [Column("TAX_NO")]
        [StringLength(20)]
        public string TaxNo { get; set; }
        [Column("VAT_NO")]
        [StringLength(20)]
        public string VatNo { get; set; }
        [Column("STREET")]
        [StringLength(250)]
        public string Street { get; set; }
        [Column("TELL_NO")]
        [StringLength(20)]
        public string TellNo { get; set; }
        [Column("COUNTRY_CODE")]
        [StringLength(2)]
        public string CountryCode { get; set; }
        [Column("PROVINCE_CODE")]
        [StringLength(2)]
        public string ProvinceCode { get; set; }
        [Column("DISTRICT_NAME")]
        [StringLength(250)]
        public string DistrictName { get; set; }
        [Column("SUBDISTRICT_NAME")]
        [StringLength(250)]
        public string SubdistrictName { get; set; }
        [Column("POST_CODE")]
        [StringLength(5)]
        public string PostCode { get; set; }
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
        [Column("DISTRICT_CODE")]
        [StringLength(10)]
        public string DistrictCode { get; set; }
        [Column("SUBDISTRICT_CODE")]
        [StringLength(10)]
        public string SubdistrictCode { get; set; }
        [Column("SYNC_ID", TypeName = "numeric(10, 0)")]
        public decimal? SyncId { get; set; }
    }
}
