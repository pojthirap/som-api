using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("MS_PLANT_PRODUCT")]
    public partial class MsPlantProduct
    {
        [Key]
        [Column("PLANT_PROD_ID", TypeName = "numeric(10, 0)")]
        public decimal PlantProdId { get; set; }
        [Required]
        [Column("PLANT_CODE")]
        [StringLength(10)]
        public string PlantCode { get; set; }
        [Required]
        [Column("PROD_CODE")]
        [StringLength(10)]
        public string ProdCode { get; set; }
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
