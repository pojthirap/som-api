using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Keyless]
    [Table("product")]
    public partial class Product
    {
        [Column("id")]
        public int? Id { get; set; }
        [Column("name")]
        [StringLength(500)]
        public string Name { get; set; }
    }
}
