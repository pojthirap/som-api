using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("ADM_LOG_LOGIN")]
    public partial class AdmLogLogin
    {
        [Key]
        [Column("LOG_LOGIN_ID", TypeName = "numeric(10, 0)")]
        public decimal LogLoginId { get; set; }
        [Required]
        [Column("USER_NAME")]
        [StringLength(20)]
        public string UserName { get; set; }
        [Column("LOGIN_DTM", TypeName = "datetime")]
        public DateTime LoginDtm { get; set; }
        [Required]
        [Column("STATUS")]
        [StringLength(1)]
        public string Status { get; set; }
        [Column("ERROR_DESCRIPTION")]
        [StringLength(500)]
        public string ErrorDescription { get; set; }
        [Required]
        [Column("IP_ADDRESS")]
        [StringLength(250)]
        public string IpAddress { get; set; }
        [Required]
        [Column("USER_AGENT")]
        [StringLength(500)]
        public string UserAgent { get; set; }
    }
}
