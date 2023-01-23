using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Football.Data;
using Football.Models;

namespace Football.Models
{
    public sealed class Player
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [RegularExpression(@"^([a-zA-ZА-Яа-я '])+$")]
        [StringLength(50, MinimumLength = 1)]
        public string Name { get; set; } 

        [Required]
        [RegularExpression(@"^([a-zA-ZА-Яа-я '])+$")]
        [StringLength(50, MinimumLength = 1)]
        public string Surname { get; set; }

        [Required]
        [Column(TypeName = "char(1)")]
        public byte Gender { get; set; }

        [Required]
        [Column(TypeName = "Date")]
        public DateTime Birthday { get; set; }

        public int TeamID { get; set; }

        [ForeignKey("TeamID")]
        public Team Team { get; set; }

        [Required]
        public int CountryID { get; set; }

        [ForeignKey("CountryID")]
        public Country Country { get; set; }

    }
}
