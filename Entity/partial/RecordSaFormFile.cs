using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Entity
{
    public partial class RecordSaFormFile
    {
        [NotMapped]
        public string FileUrl { get; set; }
        
    }
}
