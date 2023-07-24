using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("ORG_COMPANY")]
    public partial class OrgCompany
    {
        [Key]
        [Column("COMPANY_CODE")]
        [StringLength(10)]
        public string CompanyCode { get; set; }
        [Column("COMPANY_NAME_TH")]
        [StringLength(250)]
        public string CompanyNameTh { get; set; }
        [Column("COMPANY_NAME_EN")]
        [StringLength(250)]
        public string CompanyNameEn { get; set; }
        [Column("VAT_REGIST_NO")]
        [StringLength(20)]
        public string VatRegistNo { get; set; }
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
