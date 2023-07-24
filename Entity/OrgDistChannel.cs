using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("ORG_DIST_CHANNEL")]
    public partial class OrgDistChannel
    {
        [Key]
        [Column("CHANNEL_CODE")]
        [StringLength(10)]
        public string ChannelCode { get; set; }
        [Column("CHANNEL_NAME_TH")]
        [StringLength(250)]
        public string ChannelNameTh { get; set; }
        [Column("CHANNEL_NAME_EN")]
        [StringLength(250)]
        public string ChannelNameEn { get; set; }
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
