using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("PROSPECT_ACCOUNT")]
    public partial class ProspectAccount
    {
        [Key]
        [Column("PROSP_ACC_ID", TypeName = "numeric(10, 0)")]
        public decimal ProspAccId { get; set; }
        [Column("ACC_NAME")]
        [StringLength(250)]
        public string AccName { get; set; }
        [Column("BRAND_ID", TypeName = "numeric(10, 0)")]
        public decimal? BrandId { get; set; }
        [Column("CUST_CODE")]
        [StringLength(20)]
        public string CustCode { get; set; }
        [Column("IDENTIFY_ID")]
        [StringLength(20)]
        public string IdentifyId { get; set; }
        [Column("ACC_GROUP_REF")]
        [StringLength(20)]
        public string AccGroupRef { get; set; }
        [Column("REMARK")]
        [StringLength(250)]
        public string Remark { get; set; }
        [Column("SOURCE_TYPE")]
        [StringLength(1)]
        public string SourceType { get; set; }
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
