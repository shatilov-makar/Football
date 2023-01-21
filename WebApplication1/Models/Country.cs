﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace WebApplication.Models
{
    public sealed class Country
    {
        [Key]
        public Guid ID { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public ICollection<Player> Players { get; set; }


    }
}