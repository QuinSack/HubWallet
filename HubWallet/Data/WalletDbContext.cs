using HubWallet.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HubWallet.Data
{
    public class WalletDbContext : IdentityDbContext<IdentityUser>
    {
        public WalletDbContext(DbContextOptions<WalletDbContext> options) : base(options)
        {

        }

        public DbSet<Wallet> Wallets { get; set; }
    }
}
