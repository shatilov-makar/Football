using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Dto
{
    public class TeamDto
    {
        public Guid ID { get; init; }

        [Required]
        [RegularExpression(@"^([a-zA-ZА-Яа-я '])+$", ErrorMessage = "Допускается использование только букв русского и английского алфавитов")]
        public string Name { get; init; }
    }
}
