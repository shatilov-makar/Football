using System;
using System.ComponentModel.DataAnnotations;

namespace Football.Dto
{
    public class TeamDto
    {
        public int ID { get; init; }

        [Required]
        [RegularExpression(@"^([a-zA-ZА-Яа-я '])+$", ErrorMessage = "Допускается использование только букв русского и английского алфавитов")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Недопустимая длина названия команды")]
        public string Name { get; init; }
    }
}
