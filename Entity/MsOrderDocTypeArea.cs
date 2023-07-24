using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("MS_ORDER_DOC_TYPE_AREA")]
    public partial class MsOrderDocTypeArea
    {
        [Key]
        [Column("DOC_TYPE_AREA_ID", TypeName = "numeric(10, 0)")]
        public decimal DocTypeAreaId { get; set; }
        [Required]
        [Column("DOC_TYPE_CODE")]
        [StringLength(10)]
        public string DocTypeCode { get; set; }
        [Column("AREA_ID", TypeName = "numeric(10, 0)")]
        public decimal AreaId { get; set; }
        [Required]
        [Column("ACTIVE_FLAG")]
        [StringLength(1)]
        public string ActiveFlag { get; set; }
        [Required]
        [Column("CREATE_USER")]
        [StringLength(20)]
        public string CreateUser { get; set; }
        [Column("CREATE_DTM", TypeName = "datetime")]
        public DateTime CreateDtm { get; set; }
        [Required]
        [Column("UPDATE_USER")]
        [StringLength(20)]
        public string UpdateUser { get; set; }
        [Column("UPDATE_DTM", TypeName = "datetime")]
        public DateTime UpdateDtm { get; set; }
        [Column("SYNC_ID", TypeName = "numeric(10, 0)")]
        public decimal? SyncId { get; set; }
    }
}
