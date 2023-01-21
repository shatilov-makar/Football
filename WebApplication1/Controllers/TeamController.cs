using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Data;
using Microsoft.AspNetCore.JsonPatch;
using WebApplication.Models;
using WebApplication.Dto;
using System;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace WebApplication.Controllers
{
    [Route("teams")]
    public class TeamController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;


        public TeamController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Team, TeamDto>().ReverseMap());
            _mapper = new Mapper(config);
        }

        /// <summary>
        /// Returns all teams.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<TeamDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllTeams()
        {
            var teams = await _applicationDbContext.Teams.OrderBy(team => team.Name).Select(team => new {
                team.ID,
                team.Name
            }).ToListAsync();
            return Ok(teams);
        }

        /// <summary>
        /// Returns a team by its ID. If no team with given ID is found, returns 204.
        /// </summary>
        [HttpGet("{team_id:Guid}")]
        [ProducesResponseType(typeof(TeamDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetTeam(Guid team_id)
        {
            var team = await _applicationDbContext.Teams.SingleOrDefaultAsync(team => team.ID == team_id);
            if (team == null)
            {
                return NoContent();
            }
            return Ok(_mapper.Map<Team, TeamDto>(team));
        }


        /// <summary>
        /// Creates a new team. If a team with the same name already exists, returns 400.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateTeam([FromBody] TeamDto newTeam)
        {
            if (ModelState.IsValid)
            {
                var team = _mapper.Map<TeamDto, Team>(newTeam);

                if (!_applicationDbContext.Teams.Any(t => t.Name == team.Name))
                {
                    _applicationDbContext.Add(team);
                    await _applicationDbContext.SaveChangesAsync();
                    return CreatedAtAction(nameof(GetTeam), new { team.ID });
                }
                ModelState.AddModelError(nameof(TeamDto), "Эту команду уже добавляли");
            }
            return BadRequest(ModelState);

        }

    }

}

