using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("MS_SERVICE")]
    public partial class MsService
    {
        [Key]
        [Column("SERVICE_ID", TypeName = "numeric(10, 0)")]
        public decimal ServiceId { get; set; }
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
        [StringLength(50)]
        public string CreateUser { get; set; }
        [Column("CREATE_DTM", TypeName = "datetime")]
        public DateTime CreateDtm { get; set; }
        [Required]
        [Column("UPDATE_USER")]
        [StringLength(50)]
        public string UpdateUser { get; set; }
        [Column("UPDATE_DTM", TypeName = "datetime")]
        public DateTime UpdateDtm { get; set; }
    }
}
