using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Entity
{
    public partial class RecordAppFormFile
    {
        [NotMapped]
        public string FileUrl { get; set; }
        [NotMapped]
        public string EmpName { get; set; }

    }
}
