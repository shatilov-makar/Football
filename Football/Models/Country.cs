using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace Football.Models
{
    public sealed class Country
    {
        [Key]
        public int ID { get; init; }

        [Required]
        public string Name { get; init; }

        public ICollection<Player> Players { get; set; }
    }
}
