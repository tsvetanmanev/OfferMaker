namespace OfferMaker.Services.Implementations
{
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;
    using OfferMaker.Data;
    using OfferMaker.Data.Models;
    using OfferMaker.Services.Models.Account;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class AccountService : IAccountService
    {
        private readonly OfferMakerDbContext db;

        public AccountService(OfferMakerDbContext db)
        {
            this.db = db;
        }



        public async Task CreateAsync(string name, string address, string description, string managerId)
        {
            var account = new Account
            {
                Name = name,
                Address = address,
                Description = description,
                ManagerId = managerId
            };

            await this.db.Accounts.AddAsync(account);
            await this.db.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var account = await this.db.Accounts.FindAsync(id);

            if (account == null)
            {
                return false;
            }

            this.db.Accounts.Remove(account);
            await this.db.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<AccountListingServiceModel>> GetAllAsync()
            => await this.db
            .Accounts
            .ProjectTo<AccountListingServiceModel>()
            .ToListAsync();

        public async Task<AccountDetailsServiceModel> GetByIdAsync(int id)
            => await this.db
            .Accounts
            .Where(a => a.Id == id)
            .ProjectTo<AccountDetailsServiceModel>()
            .FirstOrDefaultAsync();
    }
}
