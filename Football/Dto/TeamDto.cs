using System;
using System.ComponentModel.DataAnnotations;

namespace Football.Dto
{
    public class TeamDto
    {
        public int ID { get; init; }

        [Required(ErrorMessage = "Необходимо ввести название команды")]
        [RegularExpression(@"^(([A-Za-zА-Яа-я])|([A-Za-zА-Яа-я]['-\.](?=[A-Za-zА-Яа-я]))|( (?=[A-Za-zА-Яа-я])))*$", ErrorMessage = "Недопустимое название команды")]
        [StringLength(50, ErrorMessage = "Недопустимая длина названия команды")]
        public string Name { get; init; }
    }
}
