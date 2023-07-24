using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("PROSPECT_DEDICATE_TERT")]
    public partial class ProspectDedicateTert
    {
        [Key]
        [Column("PROSP_DEDICATE_ID", TypeName = "numeric(10, 0)")]
        public decimal ProspDedicateId { get; set; }
        [Column("PROSPECT_ID", TypeName = "numeric(10, 0)")]
        public decimal ProspectId { get; set; }
        [Column("TERRITORY_ID", TypeName = "numeric(10, 0)")]
        public decimal TerritoryId { get; set; }
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
    }
}
