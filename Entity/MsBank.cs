using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("MS_BANK")]
    [Index(nameof(BankNameTh), Name = "UK_MS_BANK", IsUnique = true)]
    public partial class MsBank
    {
        [Key]
        [Column("BANK_ID", TypeName = "numeric(10, 0)")]
        public decimal BankId { get; set; }
        [Required]
        [Column("BANK_CODE")]
        [StringLength(10)]
        public string BankCode { get; set; }
        [Required]
        [Column("BANK_NAME_TH")]
        [StringLength(250)]
        public string BankNameTh { get; set; }
        [Required]
        [Column("BANK_NAME_EN")]
        [StringLength(250)]
        public string BankNameEn { get; set; }
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
