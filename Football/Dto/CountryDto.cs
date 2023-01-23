using System;
using System.ComponentModel.DataAnnotations;

namespace Football.Dto
{
    public class CountryDto
    {
        [Required]
        public int ID { get; init; }

        [Required]
        public string Name { get; init; }
    }
}
