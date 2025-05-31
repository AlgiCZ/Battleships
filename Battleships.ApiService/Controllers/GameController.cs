using Battleships.ApiService.Services;
using Battleships.ServiceDefaults.Infrastructure;
using Battleships.ServiceDefaults.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Battleships.ApiService.Controllers
{
    [ApiController]
    [Route("api/games")]
    public class GamesController : Controller
    {
        private readonly IGameService _gameService;

        public GamesController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpPost("add")]
        public ActionResult<GameResponse> Add(AddGameRequest addGameRequest)
        {
            var response = _gameService.CreateGame(addGameRequest);
            if (response == null)
            {
                return BadRequest("Failed to create game. Please check the request parameters.");
            }

            return Ok(response);
        }

        [HttpGet("{id:int}/RandomShipPositions")]
        public ActionResult<GetRandomShipPositionsResponse> GetRandomShipPositions(int id, [FromQuery] string playerName, CancellationToken ct)
        {
            var response = _gameService.GetRandomShipPositions(id, playerName);
            return response != null ? Ok(response) : NotFound($"Game with ID {id} not found or player name {playerName} is invalid.");
        }

        [HttpGet("{id:int}")]
        public ActionResult<GetGameResponse> Get(int id, [FromQuery] string playerName, CancellationToken ct)
        {
            var response = _gameService.GetGame(id, playerName);
            return response != null ? Ok(response) : NotFound($"Game with ID {id} not found or player name {playerName} is invalid.");
        }

        [HttpPut("{id:int}/ReadyPlayerStatus")]
        public ActionResult<GameStatusResponse> UpdateReadyPlayerStatus(int id, [FromQuery] string playerName)
        {
            var response = _gameService.UpdateReadyPlayerStatus(id, playerName);
            if (response == null)
            {
                return NotFound($"Game with ID {id} not found or player name {playerName} is invalid.");
            }
            return Ok(response);
        }

        [HttpGet("{id:int}/GameStatus")]
        public ActionResult<GameStatusResponse> GetGameStatus(int id, CancellationToken ct)
        {
            var response = _gameService.GetGameStatus(id);
            if (response == null)
            {
                return NotFound($"Game with ID {id} not found.");
            }
            return Ok(response);
        }

        [HttpPut("{id:int}")]
        public ActionResult<UpdateImpactsResponse> UpdateImpacts(int id, UpdateImpactsRequest request)
        {
            var games = _gameService.UpdateImpacts(id, request);
            if (games == null)
            {
                return NotFound($"Game with ID {id} not found or invalid request parameters.");
            }
            return Ok(games);
        }
    }

    
}
