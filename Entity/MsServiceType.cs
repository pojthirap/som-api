using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("MS_SERVICE_TYPE")]
    [Index(nameof(ServiceNameTh), Name = "UK_MS_SERVICE_TYPE", IsUnique = true)]
    public partial class MsServiceType
    {
        [Key]
        [Column("SERVICE_TYPE_ID", TypeName = "numeric(10, 0)")]
        public decimal ServiceTypeId { get; set; }
        [Required]
        [Column("SERVICE_CODE")]
        [StringLength(10)]
        public string ServiceCode { get; set; }
        [Required]
        [Column("SERVICE_NAME_TH")]
        [StringLength(250)]
        public string ServiceNameTh { get; set; }
        [Required]
        [Column("SERVICE_NAME_EN")]
        [StringLength(250)]
        public string ServiceNameEn { get; set; }
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
