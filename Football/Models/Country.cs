using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace Football.Models
{
    public sealed class Country
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public ICollection<Player> Players { get; set; }
    }
}
