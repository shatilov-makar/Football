using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication.Data;
using WebApplication.Models;

namespace WebApplication.Models
{
    public sealed class Player
    {
        [Key]
        public Guid ID { get; set; }


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

        public Guid TeamID { get; set; }

        [ForeignKey("TeamID")]
        public Team Team { get; set; }


        [Required]
        public Guid CountryID { get; set; }

        [ForeignKey("CountryID")]
        public Country Country { get; set; }






    }
}
