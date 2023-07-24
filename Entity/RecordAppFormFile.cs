using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("RECORD_APP_FORM_FILE")]
    public partial class RecordAppFormFile
    {
        [Key]
        [Column("REC_APP_FORM_FILE_ID", TypeName = "numeric(10, 0)")]
        public decimal RecAppFormFileId { get; set; }
        [Column("FILE_ID", TypeName = "numeric(10, 0)")]
        public decimal FileId { get; set; }
        [Column("REC_APP_FORM_ID", TypeName = "numeric(10, 0)")]
        public decimal? RecAppFormId { get; set; }
        [Column("ATTACH_CATE_ID", TypeName = "numeric(10, 0)")]
        public decimal? AttachCateId { get; set; }
        [Column("FILE_NAME")]
        [StringLength(100)]
        public string FileName { get; set; }
        [Column("FILE_EXT")]
        [StringLength(10)]
        public string FileExt { get; set; }
        [Column("FILE_SIZE")]
        [StringLength(20)]
        public string FileSize { get; set; }
        [Column("PHOTO_FLAG")]
        [StringLength(1)]
        public string PhotoFlag { get; set; }
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
    }
}
