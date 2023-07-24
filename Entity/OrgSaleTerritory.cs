using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("ORG_SALE_TERRITORY")]
    public partial class OrgSaleTerritory
    {
        [Key]
        [Column("SALE_TERRITORY_ID", TypeName = "numeric(10, 0)")]
        public decimal SaleTerritoryId { get; set; }
        [Column("TERRITORY_ID", TypeName = "numeric(10, 0)")]
        public decimal? TerritoryId { get; set; }
        [Column("EMP_ID")]
        [StringLength(20)]
        public string EmpId { get; set; }
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
        [Column("ACTIVE_FLAG")]
        [StringLength(1)]
        public string ActiveFlag { get; set; }
    }
}
