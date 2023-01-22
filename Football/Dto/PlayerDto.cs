using System;
using System.ComponentModel.DataAnnotations;

namespace Football.Dto
{
    public class PlayerDto
    {
        public Guid ID { get; init; }


        [Required]
        [RegularExpression(@"^([a-zA-ZА-Яа-я '])+$",
      ErrorMessage = "Допускается использование только букв русского и английского алфавитов")]
        [StringLength(50, MinimumLength =1, ErrorMessage="Недопустимая длина имени")]
        public string Name { get; init; }

        [Required]
        [RegularExpression(@"^([a-zA-ZА-Яа-я '])+$",
  ErrorMessage = "Допускается использование только букв русского и английского алфавитов")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Недопустимая длина фамилии")]
        public string Surname { get; init; }

        [Required]
        public DateTime  Birthday { get; init; }

        [Required]
        [Range(0, 1)]
        public byte Gender { get; init; }

        public string TeamName { get; init; }
       

        public Guid TeamID { get; init; }

        public string CountryName { get; init; }

        public Guid CountryID { get; init; }


    }
}
