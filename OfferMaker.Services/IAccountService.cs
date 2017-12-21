namespace OfferMaker.Services
{
    using OfferMaker.Services.Models.Account;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IAccountService
    {
        Task<IEnumerable<AccountListingServiceModel>> GetAllAsync();

        Task CreateAsync(string name, string address, string description, string managerId);

        Task<AccountDetailsServiceModel> GetById(int id);
    }
}
