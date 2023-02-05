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
        public int ID { get; init; }

        [Required]
        [RegularExpression(@"^(([A-Za-zА-Яа-я])|([A-Za-zА-Яа-я]['-\.](?=[A-Za-zА-Яа-я]))|( (?=[A-Za-zА-Яа-я])))*$")]
        [StringLength(50)]
        public string Name { get; set; } 

        [Required]
        [RegularExpression(@"^(([A-Za-zА-Яа-я])|([A-Za-zА-Яа-я]['-\.](?=[A-Za-zА-Яа-я]))|( (?=[A-Za-zА-Яа-я])))*$")]
        [StringLength(50)]
        public string Surname { get; set; }

        [Required]
        [Column(TypeName = "char(1)")]
        [Range(0, 1)]
        public byte Gender { get; set; }

        [Required]
        [Column(TypeName = "Date")]
        public DateTime Birthday { get; set; }

        [Required]
        public int TeamID { get; set; }

        [ForeignKey("TeamID")]
        public Team Team { get; set; }

        [Required]
        public int CountryID { get; set; }

        [ForeignKey("CountryID")]
        public Country Country { get; set; }

    }
}
