using Microsoft.AspNetCore.Mvc;
using WorldRank.Api.DTO;
using WorldRank.Application.Services;
using WorldRank.Domain.Exceptions;

namespace WorldRank.Api.Controllers
{
    [ApiController]
    [Route("wallets")]
    public class WalletsController : ControllerBase
    {
        private readonly WalletService _wallets;

        public WalletsController(WalletService wallets) => _wallets = wallets;

        // POST /wallets — create a wallet, return 201 Created with a Location header.
        // The service writes the new wallet through to the cache and drops the list views.
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateWalletRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var wallet = await _wallets.CreateWalletAsync(request.PlayerId, request.Currency, request.InitialBalance, cancellationToken);
                return CreatedAtAction(nameof(GetById), new { id = wallet.Id }, WalletResponse.From(wallet));
            }
            catch (PlayerNotFoundException ex)
            {
                // Cannot create a wallet for a player that does not exist → 404.
                return NotFound(new { error = ex.Message });
            }
            catch (WalletException ex)
            {
                // Duplicate wallet / negative initial balance → 400.
                return BadRequest(new { error = ex.Message });
            }
        }

        // GET /wallets/{id} — 200 or 404 (cache-aside with 60s TTL inside WalletService).
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var wallet = await _wallets.GetByIdAsync(id, cancellationToken);
            return wallet is null ? NotFound() : Ok(WalletResponse.From(wallet));
        }

        // POST /wallets/{id}/deposit — deposit funds (the service writes the fresh wallet
        // through to the cache and invalidates the list caches on success).
        [HttpPost("{id:int}/deposit")]
        public async Task<IActionResult> Deposit(int id, [FromBody] DepositRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var wallet = await _wallets.DepositAsync(id, request.Amount, cancellationToken);
                return wallet is null ? NotFound() : Ok(WalletResponse.From(wallet));
            }
            catch (WalletException ex)
            {
                // Business-rule violation (blocked wallet, non-positive amount) → 400.
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
