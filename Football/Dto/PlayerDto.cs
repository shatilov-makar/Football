using System;
using System.ComponentModel.DataAnnotations;

namespace Football.Dto
{
    public class PlayerDto
    {
        public int ID { get; init; }

        [Required(ErrorMessage="Необходимо ввести имя")]
        [RegularExpression(@"^(([A-Za-zА-Яа-я])|([A-Za-zА-Яа-я]['-\.](?=[A-Za-zА-Яа-я]))|( (?=[A-Za-zА-Яа-я])))*$", ErrorMessage = "Недопустимое имя")]
        [StringLength(50, ErrorMessage="Недопустимая длина имени")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Необходимо ввести фамилию")]
        [RegularExpression(@"^(([A-Za-zА-Яа-я])|([A-Za-zА-Яа-я]['-\.](?=[A-Za-zА-Яа-я]))|( (?=[A-Za-zА-Яа-я])))*$", ErrorMessage = "Недопустимая фамилия")]
        [StringLength(50, ErrorMessage = "Недопустимая длина фамилии")]
        public string Surname { get; set; }

        [Required]
        public DateTime  Birthday { get; set; }

        [Required]
        [Range(0, 1)]
        public byte Gender { get; set; }

        public string TeamName { get; set; }

        [Required]
        public int TeamID { get; set; }

        public string CountryName { get; set; }

        [Required]
        public int CountryID { get; set; }
    }
}
