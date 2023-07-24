using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("CUSTOMER_PARTNER")]
    public partial class CustomerPartner
    {
        [Key]
        [Column("CUST_PARTNER_ID", TypeName = "numeric(10, 0)")]
        public decimal CustPartnerId { get; set; }
        [Column("CUST_CODE")]
        [StringLength(20)]
        public string CustCode { get; set; }
        [Column("ORG_CODE")]
        [StringLength(10)]
        public string OrgCode { get; set; }
        [Column("CHANNEL_CODE")]
        [StringLength(10)]
        public string ChannelCode { get; set; }
        [Column("DIVISION_CODE")]
        [StringLength(10)]
        public string DivisionCode { get; set; }
        [Column("FUNC_CODE")]
        [StringLength(2)]
        public string FuncCode { get; set; }
        [Column("CUST_CODE_PARTNER")]
        [StringLength(20)]
        public string CustCodePartner { get; set; }
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
        [Column("PARTNER_COUNTER")]
        [StringLength(20)]
        public string PartnerCounter { get; set; }
    }
}
