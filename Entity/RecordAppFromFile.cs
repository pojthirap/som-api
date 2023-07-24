using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("RECORD_APP_FROM_FILE")]
    [Index(nameof(AttachCateId), Name = "RELATIONSHIP_109_FK")]
    [Index(nameof(RecAppFormId), Name = "RELATIONSHIP_110_FK")]
    public partial class RecordAppFromFile
    {
        [Key]
        [Column("FILE_ID", TypeName = "numeric(10, 0)")]
        public decimal FileId { get; set; }
        [Column("ATTACH_CATE_ID", TypeName = "numeric(10, 0)")]
        public decimal? AttachCateId { get; set; }
        [Column("REC_APP_FORM_ID", TypeName = "numeric(10, 0)")]
        public decimal RecAppFormId { get; set; }
        [Required]
        [Column("FILE_NAME")]
        [StringLength(20)]
        public string FileName { get; set; }
        [Required]
        [Column("FILE_EXT")]
        [StringLength(10)]
        public string FileExt { get; set; }
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
