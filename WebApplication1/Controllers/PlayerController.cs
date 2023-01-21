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
using System.Numerics;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace WebApplication.Controllers
{
    [Route("players")]
    public class PlayerController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;


        public PlayerController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
            var config = new MapperConfiguration(cfg => cfg.CreateMap<PlayerDto, Player>()
            .ReverseMap()
            .ForMember(dest => dest.TeamName, y => y.MapFrom(c => c.Team.Name))
            .ForMember(dest => dest.CountryName, y => y.MapFrom(c => c.Country.Name))
            .ForMember(dest => dest.Birthday, y => y.MapFrom(c => DateTime.Parse(c.Birthday.ToString("dd.MM.yyyy")))));

            _mapper = new Mapper(config);
        }

        /// <summary>
        /// Returns a player by his ID. If no player with given ID is found, returns 204.
        /// </summary>
        [HttpGet("{player_id:Guid}")]
        [ProducesResponseType(typeof(PlayerDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetPlayer(Guid player_id)
        {
            var player = await _applicationDbContext.Players
                .Include(player => player.Team)
                .Include(player => player.Country)
                .SingleOrDefaultAsync(player => player.ID == player_id);
           if (player == null)
            {
                return NoContent();
            }
            return Ok(_mapper.Map<Player, PlayerDto>(player));
        }


        /// <summary>
        /// Returns all players.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<PlayerDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllPlayers()
        {
            var players = await _applicationDbContext.Players
                .OrderBy(player => player.Surname)
                .Include(player => player.Team)
                .Include(player => player.Country)
                .Select(player => _mapper.Map<Player, PlayerDto>(player))
                .ToListAsync();
            return Ok(players);
        }

        /// <summary>
        /// Creates a new player. If a player with the same name, surname and birthday already exists, returns 400.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType( StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreatePlayer([FromBody] PlayerDto newPlayer)
        {
            if (ModelState.IsValid)
            {
                if (_applicationDbContext.Players.Any(p => p.Name == newPlayer.Name && p.Surname == newPlayer.Surname && p.Birthday == newPlayer.Birthday))
                {
                    ModelState.AddModelError(nameof(PlayerDto), "Этого игрока уже добавляли");
                    return BadRequest(ModelState);
                }
                var player = _mapper.Map<PlayerDto, Player>(newPlayer);

                _applicationDbContext.Add(player);
                _applicationDbContext.Entry(player).Reference(p => p.Team).Load();
                _applicationDbContext.Entry(player).Reference(p => p.Country).Load();
                await _applicationDbContext.SaveChangesAsync();
                return CreatedAtAction(nameof(GetPlayer), new { player.ID });
            }
            return BadRequest(ModelState);
        }

        /// <summary>
        /// Updates a player. If no player with given ID is found, returns 400.
        /// </summary
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePlayer([FromBody] PlayerDto updatedPlayer)
        {
            if (ModelState.IsValid)
            {
                if (_applicationDbContext.Players.Any(p => p.ID == updatedPlayer.ID))
                {
                    var player = _mapper.Map<PlayerDto, Player>(updatedPlayer);
                    _applicationDbContext.Players.Update(player);
                    _applicationDbContext.Entry(player).Reference(p => p.Team).Load();
                    _applicationDbContext.Entry(player).Reference(p => p.Country).Load();
                    await _applicationDbContext.SaveChangesAsync();
                    return NoContent();
                }
                ModelState.AddModelError(nameof(PlayerDto), "Игрока с таким идентификатором нет");
            }
            return BadRequest(ModelState);
        }
    }
}

