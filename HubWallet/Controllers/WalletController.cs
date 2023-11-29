using HubWallet.Data;
using HubWallet.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HubWallet.Controllers
{
    [Route("api/wallets")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly WalletDbContext _dbContext;
        public WalletController(WalletDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Wallet))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> AddWallet(Wallet wallet)
        {
            //check for duplicate wallets
            if(_dbContext.Wallets.Any(w => 
            w.Owner == wallet.Owner && 
            w.Type == wallet.Type &&
            w.AccountNumber == wallet.AccountNumber &&
            w.AccountScheme == wallet.AccountScheme
            ))
            {
                return Conflict("Duplicate wallet");
            }

            //check maximum wallets per user
            if(_dbContext.Wallets.Count(w => w.Owner == wallet.Owner) >= 5)
            {
                return BadRequest("Maximum 5 wallets per user allowed");
            }

            //store only the first 6 digits of the card number
            if(wallet.Type == "card" && !string.IsNullOrEmpty(wallet.AccountNumber))
            {
                wallet.AccountNumber = wallet.AccountNumber.Substring(0, Math.Min(6, wallet.AccountNumber.Length));
            }

            _dbContext.Wallets.Add(wallet);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetWalletById), new {id =  wallet.Id}, wallet);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveWallet(int id)
        {
            var wallet = await _dbContext.Wallets.FindAsync(id);
            if(wallet == null)
            {
                return NotFound();
            }

            _dbContext.Wallets.Remove(wallet);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Wallet))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetWalletById(int id)
        {
            var wallet = await _dbContext.Wallets.FindAsync(id);
             
            if (wallet == null)
            {
                return NotFound();
            }

            return Ok(wallet);
        }

        [HttpGet("owner/{phoneNumber}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Wallet>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetWalletsByOwner(string phoneNumber)
        {
            var wallets = _dbContext.Wallets.Where(w => w.Owner == phoneNumber).ToList();

            if (wallets.Count == 0)
            {
                return NotFound($"No wallets found for owner with phone number {phoneNumber}");
            }

            return Ok(wallets);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Wallet>))]
        public async Task<IActionResult> GetAllWallets()
        {
            var wallets = await _dbContext.Wallets.ToListAsync();
            return Ok(wallets);
        }
    }
}
