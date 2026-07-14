using Microsoft.AspNetCore.Mvc;
using WorldRank.Api.DTO;
using WorldRank.Application.Services;

namespace WorldRank.Api.Controllers
{
    [ApiController]
    [Route("players")]
    public class PlayersController : ControllerBase
    {
        private readonly PlayerService _players;

        public PlayersController(PlayerService players) => _players = players;

        // POST /players — create a player, return 201 Created with a Location header.
        // The service writes the new player through to the cache.
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePlayerRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var player = await _players.CreateAsync(request.Name, request.Score, cancellationToken);
                return CreatedAtAction(nameof(GetById), new { id = player.Id }, PlayerResponse.From(player));
            }
            catch (ArgumentException ex)
            {
                // Empty name / negative score (ArgumentOutOfRangeException derives from this) → 400.
                return BadRequest(new { error = ex.Message });
            }
        }

        // GET /players/{id} — 200 or 404 (cache-aside inside PlayerService).
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var player = await _players.GetByIdAsync(id, cancellationToken);
            return player is null ? NotFound() : Ok(PlayerResponse.From(player));
        }

        // GET /players — list all players (200, cached for 60s).
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var players = await _players.GetAllAsync(cancellationToken);
            return Ok(players.Select(PlayerResponse.From));
        }
    }
}
