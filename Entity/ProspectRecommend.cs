using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("PROSPECT_RECOMMEND")]
    public partial class ProspectRecommend
    {
        [Key]
        [Column("PROSP_RECOMM_ID", TypeName = "numeric(10, 0)")]
        public decimal ProspRecommId { get; set; }
        [Column("PROSPECT_ID", TypeName = "numeric(10, 0)")]
        public decimal? ProspectId { get; set; }
        [Column("BU_ID", TypeName = "numeric(10, 0)")]
        public decimal? BuId { get; set; }
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
