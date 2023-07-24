using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("PROSPECT_FEED")]
    public partial class ProspectFeed
    {
        [Key]
        [Column("FEED_ID", TypeName = "numeric(10, 0)")]
        public decimal FeedId { get; set; }
        [Column("PROSPECT_ID", TypeName = "numeric(10, 0)")]
        public decimal ProspectId { get; set; }
        [Column("FUNCTION_TAB", TypeName = "numeric(2, 0)")]
        public decimal FunctionTab { get; set; }
        [Required]
        [Column("DESCRIPTION", TypeName = "text")]
        public string Description { get; set; }
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
