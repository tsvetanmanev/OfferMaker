namespace OfferMaker.Services
{
    using OfferMaker.Services.Models.Account;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IAccountService
    {
        Task<IEnumerable<AccountListingServiceModel>> GetAllAsync();

        Task CreateAsync(string name, string address, string description, string managerId);

        Task<AccountDetailsServiceModel> GetByIdAsync(int id);

        Task<bool> DeleteAsync(int id);

        Task<bool> UserIsAssignedAccountManager(string userId, int accountId);

        Task EditAsync(int id, string name, string address, string description);
    }
}
