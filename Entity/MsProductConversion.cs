using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("MS_PRODUCT_CONVERSION")]
    public partial class MsProductConversion
    {
        [Key]
        [Column("PROD_CONV_ID", TypeName = "numeric(10, 0)")]
        public decimal ProdConvId { get; set; }
        [Column("PROD_CODE")]
        [StringLength(20)]
        public string ProdCode { get; set; }
        [Column("ALT_UNIT")]
        [StringLength(20)]
        public string AltUnit { get; set; }
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
        [Column("DENOMINATOR", TypeName = "numeric(15, 5)")]
        public decimal? Denominator { get; set; }
        [Column("COUNTER", TypeName = "numeric(15, 5)")]
        public decimal? Counter { get; set; }
        [Column("GROSS_WEIGHT", TypeName = "decimal(15, 5)")]
        public decimal? GrossWeight { get; set; }
        [Column("WEIGHT_UNIT")]
        [StringLength(25)]
        public string WeightUnit { get; set; }
        [Column("SYNC_ID", TypeName = "numeric(10, 0)")]
        public decimal? SyncId { get; set; }
    }
}
