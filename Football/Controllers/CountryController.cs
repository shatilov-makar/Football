using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Football.Data;
using Microsoft.AspNetCore.JsonPatch;
using Football.Models;
using Football.Dto;
using System;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using AutoMapper;

namespace Football.Controllers
{
    [Route("api/countries")]
    public class CountryController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;

        public CountryController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Country, CountryDto>());
            _mapper = new Mapper(config);

        }


        /// <summary>
        /// Returns all countries.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<CountryDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllCountries()
        {
            var countries = await _applicationDbContext.Countries
                .OrderBy(country => country.Name)
                .Select(country => _mapper.Map<Country, CountryDto>(country))
                .ToListAsync();
            return Ok(countries);
        }

    }

}

