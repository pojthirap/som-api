using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("MS_PRODUCT")]
    public partial class MsProduct
    {
        [Key]
        [Column("PROD_CODE")]
        [StringLength(20)]
        public string ProdCode { get; set; }
        [Column("DIVISION_CODE")]
        [StringLength(10)]
        public string DivisionCode { get; set; }
        [Column("PROD_NAME_TH")]
        [StringLength(250)]
        public string ProdNameTh { get; set; }
        [Column("PROD_NAME_EN")]
        [StringLength(250)]
        public string ProdNameEn { get; set; }
        [Column("PROD_TYPE")]
        [StringLength(20)]
        public string ProdType { get; set; }
        [Column("PROD_GROUP")]
        [StringLength(20)]
        public string ProdGroup { get; set; }
        [Column("INDUSTRY_SECTOR")]
        [StringLength(1)]
        public string IndustrySector { get; set; }
        [Column("OLD_PROD_NO")]
        [StringLength(20)]
        public string OldProdNo { get; set; }
        [Column("BASE_UNIT")]
        [StringLength(20)]
        public string BaseUnit { get; set; }
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
        [Column("REPORT_PROD_CONV_ID", TypeName = "numeric(10, 0)")]
        public decimal? ReportProdConvId { get; set; }
        [Column("SYNC_ID", TypeName = "numeric(10, 0)")]
        public decimal? SyncId { get; set; }
    }
}
