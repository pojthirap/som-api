using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("ADM_EMPLOYEE")]
    public partial class AdmEmployee
    {
        [Key]
        [Column("EMP_ID")]
        [StringLength(20)]
        public string EmpId { get; set; }
        [Column("COMPANY_CODE")]
        [StringLength(10)]
        public string CompanyCode { get; set; }
        [Column("JOB_TITLE")]
        [StringLength(50)]
        public string JobTitle { get; set; }
        [Column("TITLE_NAME")]
        [StringLength(50)]
        public string TitleName { get; set; }
        [Column("FIRST_NAME")]
        [StringLength(200)]
        public string FirstName { get; set; }
        [Column("LAST_NAME")]
        [StringLength(200)]
        public string LastName { get; set; }
        [Column("GENDER")]
        [StringLength(1)]
        public string Gender { get; set; }
        [Column("STREET")]
        [StringLength(250)]
        public string Street { get; set; }
        [Column("TELL_NO")]
        [StringLength(20)]
        public string TellNo { get; set; }
        [Column("COUNTRY_NAME")]
        [StringLength(25)]
        public string CountryName { get; set; }
        [Column("PROVINCE_CODE")]
        [StringLength(10)]
        public string ProvinceCode { get; set; }
        [Column("GROUP_CODE")]
        [StringLength(10)]
        public string GroupCode { get; set; }
        [Column("DISTRICT_NAME")]
        [StringLength(250)]
        public string DistrictName { get; set; }
        [Column("SUBDISTRICT_NAME")]
        [StringLength(250)]
        public string SubdistrictName { get; set; }
        [Column("POST_CODE")]
        [StringLength(5)]
        public string PostCode { get; set; }
        [Column("EMAIL")]
        [StringLength(250)]
        public string Email { get; set; }
        [Column("STATUS")]
        [StringLength(1)]
        public string Status { get; set; }
        [Column("APPROVE_EMP_ID")]
        [StringLength(20)]
        public string ApproveEmpId { get; set; }
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
        [Column("PROVINCE_NAME")]
        [StringLength(250)]
        public string ProvinceName { get; set; }
    }
}
