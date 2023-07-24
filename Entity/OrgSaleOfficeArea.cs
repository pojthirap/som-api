using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("ORG_SALE_OFFICE_AREA")]
    public partial class OrgSaleOfficeArea
    {
        [Key]
        [Column("OFFICE_AREA_ID", TypeName = "numeric(10, 0)")]
        public decimal OfficeAreaId { get; set; }
        [Column("OFFICE_CODE")]
        [StringLength(10)]
        public string OfficeCode { get; set; }
        [Column("AREA_ID", TypeName = "numeric(10, 0)")]
        public decimal? AreaId { get; set; }
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
        [Column("ORG_CODE")]
        [StringLength(10)]
        public string OrgCode { get; set; }
        [Column("CHANNEL_CODE")]
        [StringLength(10)]
        public string ChannelCode { get; set; }
        [Column("DIVISION_CODE")]
        [StringLength(10)]
        public string DivisionCode { get; set; }
        [Column("SYNC_ID", TypeName = "numeric(10, 0)")]
        public decimal? SyncId { get; set; }
    }
}
