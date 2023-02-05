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
        public int ID { get; init; }

        [Required]
        [RegularExpression(@"^(([A-Za-zА-Яа-я])|([A-Za-zА-Яа-я]['-\.](?=[A-Za-zА-Яа-я]))|( (?=[A-Za-zА-Яа-я])))*$")]
        public string Name { get; init; }

        public ICollection<Player> Players { get; set; }
    }
}
