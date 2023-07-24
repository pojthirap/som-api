using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("MS_SHIP")]
    public partial class MsShip
    {
        [Key]
        [Column("SHIP_CODE")]
        [StringLength(10)]
        public string ShipCode { get; set; }
        [Column("DESCRIPTION_TH")]
        [StringLength(250)]
        public string DescriptionTh { get; set; }
        [Column("DESCRIPTION_EN")]
        [StringLength(250)]
        public string DescriptionEn { get; set; }
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
