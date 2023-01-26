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

namespace Football.Hubs
{
    public class PlayersHub : Hub<IPlayersHubClient>
    {
        [HubMethodName("SendPlayerToUsers")]
        public async Task SendPlayerToUsers(PlayerDto player)
        {
            await Clients.All.SendPlayerToUsers(player);
        }

    }
}
