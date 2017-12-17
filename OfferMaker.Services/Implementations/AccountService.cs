namespace OfferMaker.Services.Implementations
{
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;
    using OfferMaker.Data;
    using OfferMaker.Services.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class AccountService : IAccountService
    {
        private readonly OfferMakerDbContext db;

        public AccountService(OfferMakerDbContext db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<AccountListingServiceModel>> GetAllAsync()
            => await this.db
            .Accounts
            .ProjectTo<AccountListingServiceModel>()
            .ToListAsync();

    }
}
