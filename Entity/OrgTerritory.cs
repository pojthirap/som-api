using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("ORG_TERRITORY")]
    [Index(nameof(TerritoryNameTh), Name = "UK_ORG_TERRITORY", IsUnique = true)]
    public partial class OrgTerritory
    {
        [Key]
        [Column("TERRITORY_ID", TypeName = "numeric(10, 0)")]
        public decimal TerritoryId { get; set; }
        [Column("TERRITORY_CODE")]
        [StringLength(10)]
        public string TerritoryCode { get; set; }
        [Required]
        [Column("TERRITORY_NAME_TH")]
        [StringLength(250)]
        public string TerritoryNameTh { get; set; }
        [Required]
        [Column("TERRITORY_NAME_EN")]
        [StringLength(250)]
        public string TerritoryNameEn { get; set; }
        [Column("MANAGER_EMP_ID")]
        [StringLength(20)]
        public string ManagerEmpId { get; set; }
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
        [Column("BU_ID", TypeName = "numeric(15, 5)")]
        public decimal? BuId { get; set; }
    }
}
