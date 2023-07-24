using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("CUSTOMER_COMPANY")]
    public partial class CustomerCompany
    {
        [Key]
        [Column("CUST_COM_ID", TypeName = "numeric(10, 0)")]
        public decimal CustComId { get; set; }
        [Column("CUST_CODE")]
        [StringLength(20)]
        public string CustCode { get; set; }
        [Column("COMPANY_CODE")]
        [StringLength(10)]
        public string CompanyCode { get; set; }
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
