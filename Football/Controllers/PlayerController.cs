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
using System.Numerics;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.AspNetCore.SignalR;
using Football.Hubs;
using Football.Parameters;
using Football.Controllers.PagedList;

namespace Football.Controllers
{
    [Route("api/players")]
    public class PlayerController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;
        private readonly IHubContext<PlayersHub, IPlayersHubClient> _playersHub;

        public PlayerController(IHubContext<PlayersHub, IPlayersHubClient> playersHub, ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
            var config = new MapperConfiguration(cfg => cfg.CreateMap<PlayerDto, Player>()
            .ReverseMap()
            .ForMember(dest => dest.TeamName, y => y.MapFrom(c => c.Team.Name))
            .ForMember(dest => dest.CountryName, y => y.MapFrom(c => c.Country.Name))
            .ForMember(dest => dest.Birthday, y => y.MapFrom(c => DateTime.Parse(c.Birthday.ToString("dd.MM.yyyy"))))
            );

            _mapper = new Mapper(config);
            _playersHub = playersHub;
        }

        /// <summary>
        /// Returns a player by his ID. If no player with given ID is found, returns 204.
        /// </summary>
        [HttpGet("{player_id:int}")]
        [ProducesResponseType(typeof(PlayerDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetPlayer(int player_id)
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
        /// Returns paginated players.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<PlayerDto>), StatusCodes.Status200OK)]
        public IActionResult GetPaginatedPlayers([FromQuery] PlayerParameters playerParameters)
        {
            var allPlayers =  _applicationDbContext.Players
                .OrderBy(player => player.ID)
                .Include(player => player.Team)
                .Include(player => player.Country)
                .Select(player => _mapper.Map<Player, PlayerDto>(player));

            var pagedPlayers = PagedList<PlayerDto>.ToPagedList(allPlayers, playerParameters.PageNumber, playerParameters.PageSize);

            var metadata = new
            {
                pagedPlayers.TotalCount,
                pagedPlayers.PageSize,
                pagedPlayers.CurrentPage,
                pagedPlayers.TotalPages,
                pagedPlayers.HasNext,
                pagedPlayers.HasPrevious
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            return Ok(pagedPlayers);
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
                    ModelState.AddModelError(nameof(PlayerDto), "????? ?????? ??? ?????????");
                    return BadRequest(ModelState);
                }
                var player = _mapper.Map<PlayerDto, Player>(newPlayer);

                _applicationDbContext.Add(player);
                _applicationDbContext.Entry(player).Reference(p => p.Team).Load();
                _applicationDbContext.Entry(player).Reference(p => p.Country).Load();
                await _applicationDbContext.SaveChangesAsync();
                await _playersHub.Clients.All.SendPlayerToUsers(_mapper.Map<Player, PlayerDto>(player));
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
                    if (_applicationDbContext.Players.Any(p => p.Name == player.Name && p.Surname == player.Surname 
                    && p.Birthday == player.Birthday && p.ID != player.ID))
                    {
                        ModelState.AddModelError(nameof(PlayerDto), "???? ????? ??? ??????????");
                        return BadRequest(ModelState);
                    }
                    _applicationDbContext.Players.Update(player);
                    _applicationDbContext.Entry(player).Reference(p => p.Team).Load();
                    _applicationDbContext.Entry(player).Reference(p => p.Country).Load();
                    await _applicationDbContext.SaveChangesAsync();
                    await _playersHub.Clients.All.SendPlayerToUsers(_mapper.Map<Player, PlayerDto>(player));
                    return NoContent();
                }
                ModelState.AddModelError(nameof(PlayerDto), "?????? ? ????? ??????????????? ???");
            }
            return BadRequest(ModelState);
        }
    }
}

