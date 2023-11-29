using HubWallet.Data;
using HubWallet.Models;
using HubWallet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HubWallet.Controllers
{
    [Authorize]
    [Route("api/wallets")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly WalletService _walletService;
        private readonly WalletUserService _walletUserService;

        public WalletController(WalletService walletService, WalletUserService walletUserService)
        {
            _walletService = walletService;
            _walletUserService = walletUserService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterUser(WalletUser walletUser)
        {
            var result = await _walletUserService.RegisterUser(walletUser);

            if (result)
            {
                return CreatedAtAction(nameof(Login), new { phoneNumber = walletUser.PhoneNumber }, walletUser);
            }

            return BadRequest("User registration failed");
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login(WalletUser walletUser)
        {
            var token = await _walletUserService.Login(walletUser);

            if (token != null)
            {
                return Ok(new { Token = token });
            }

            return Unauthorized("Invalid credentials");
        }

        //[Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> AddWallet(Wallet wallet)
        {
            var result = await _walletService.AddWallet(wallet);

            if (!result)
            {
                return Conflict("Duplicate wallet or maximum wallets per user exceeded");
            }

            return CreatedAtAction(nameof(GetWalletById), new { id = wallet.Id }, wallet);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveWallet(int id)
        {
            var result = await _walletService.RemoveWallet(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetWalletById(int id)
        {
            var wallet = await _walletService.GetWalletById(id);

            if (wallet == null)
            {
                return NotFound();
            }

            return Ok(wallet);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllWallets()
        {
            var wallets = _walletService.GetAllWallets().ToList();
            return Ok(wallets);
        }

        [HttpGet("owner/{phoneNumber}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetWalletsByOwner(string phoneNumber)
        {
            var wallets = _walletService.GetWalletsByOwner(phoneNumber).ToList();

            if (wallets.Count == 0)
            {
                return NotFound($"No wallets found for owner with phone number {phoneNumber}");
            }

            return Ok(wallets);
        }
    }
}
