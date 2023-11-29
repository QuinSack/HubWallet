using HubWallet.Models;
using Microsoft.EntityFrameworkCore;

namespace HubWallet.Data
{
    public class WalletDbContext : DbContext
    {
        public WalletDbContext(DbContextOptions<WalletDbContext> options) : base(options)
        {

        }

        public DbSet<Wallet> Wallets { get; set; }
    }
}
