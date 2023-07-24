using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Entity
{
    public partial class TemplateSaTitle
    {
        [NotMapped]
        public string title_type { get; set; }
        [NotMapped]
        public decimal title_data { get; set; }
        [NotMapped]
        public string titleColmAns { get; set; }
        [NotMapped]
        public string titleColmImagUrl { get; set; }
    }
}
