namespace HubWallet.Services;

using HubWallet.Data;
using HubWallet.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

public class WalletService
{
    private readonly WalletDbContext _dbContext;

    public WalletService(WalletDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> AddWallet(Wallet wallet)
    {
        if (await AccountNumberExists(wallet?.AccountNumber))
        {
            return false; //Duplicate wallet
        }

        //if (_dbContext.Wallets.Any(w =>
        //    w.Owner == wallet.Owner &&
        //    w.Type == wallet.Type &&
        //    w.AccountNumber == wallet.AccountNumber &&
        //    w.AccountScheme == wallet.AccountScheme))
        //{
        //    return false; // Duplicate wallet
        //}

        if (_dbContext.Wallets.Count(w => w.Owner == wallet.Owner) >= 5)
        {
            return false; // Maximum 5 wallets per user allowed
        }

        if (wallet.Type == "card" && !string.IsNullOrEmpty(wallet.AccountNumber))
        {
            wallet.AccountNumber = wallet.AccountNumber.Substring(0, Math.Min(6, wallet.AccountNumber.Length));
        }

        _dbContext.Wallets.Add(wallet);
        await _dbContext.SaveChangesAsync();
        return true; // Wallet added successfully
    }

    public async Task<bool> RemoveWallet(int id)
    {
        var wallet = await _dbContext.Wallets.FindAsync(id);

        if (wallet == null)
        {
            return false; // Wallet not found
        }

        _dbContext.Wallets.Remove(wallet);
        await _dbContext.SaveChangesAsync();
        return true; // Wallet removed successfully
    }

    public async Task<Wallet> GetWalletById(int id)
    {
        return await _dbContext.Wallets.FindAsync(id);
    }

    public IQueryable<Wallet> GetAllWallets()
    {
        return _dbContext.Wallets.AsQueryable();
    }

    public IQueryable<Wallet> GetWalletsByOwner(string phoneNumber)
    {
        return _dbContext.Wallets.Where(w => w.Owner == phoneNumber);
    }

    public async Task<bool> AccountNumberExists(string accountNumber)
    {
        return await _dbContext.Wallets.AnyAsync(w => w.AccountNumber == accountNumber);
    }
}
