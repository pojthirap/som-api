using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("ADM_SESSION")]
    public partial class AdmSession
    {
        [Key]
        [Column("SESSION_ID", TypeName = "numeric(10, 0)")]
        public decimal SessionId { get; set; }
        [Required]
        [Column("TOKEN_NO")]
        [StringLength(250)]
        public string TokenNo { get; set; }
        [Required]
        [Column("EMP_ID")]
        [StringLength(20)]
        public string EmpId { get; set; }
        [Column("IP_ADDRESS")]
        [StringLength(100)]
        public string IpAddress { get; set; }
        [Column("USER_AGENT")]
        [StringLength(250)]
        public string UserAgent { get; set; }
        [Column("LAST_ACCESS_DTM", TypeName = "datetime")]
        public DateTime? LastAccessDtm { get; set; }
        [Column("VALID")]
        [StringLength(1)]
        public string Valid { get; set; }
        [Column("LOGIN_DTM", TypeName = "datetime")]
        public DateTime? LoginDtm { get; set; }
        [Column("LOGOUT_DTM", TypeName = "datetime")]
        public DateTime? LogoutDtm { get; set; }
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
        [Column("TIMEOUT_DTM", TypeName = "datetime")]
        public DateTime? TimeoutDtm { get; set; }
    }
}
