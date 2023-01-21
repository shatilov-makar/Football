using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Dto
{
    public class CountryDto
    {
        [Required]
        public Guid ID { get; init; }

        [Required]
        public string Name { get; init; }



    }
}
