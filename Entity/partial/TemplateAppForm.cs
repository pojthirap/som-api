using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Entity
{
    public partial class TemplateAppForm
    {


        [Column("EDIT_FLAG")]
        [StringLength(10)]
        public string EditFlag { get; set; }

    }
}
