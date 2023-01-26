using Football.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Football.Hubs
{
    public interface IPlayersHubClient
    {
        Task SendPlayerToUsers(PlayerDto player);
    }
}
