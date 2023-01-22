using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Security.Policy;

namespace Football.Models
{
    public sealed class Team
    {
        [Key]
        public Guid ID { get; set; }

        [Required]
        [RegularExpression(@"^([a-zA-ZА-Яа-я '])+$")]
        public string Name { get; set; } = string.Empty;

        public ICollection<Player> Players { get; set; }

    }
}
