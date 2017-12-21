namespace OfferMaker.Services
{
    using OfferMaker.Services.Models.Opportunity;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IOpportunityService
    {
        Task CreateAsync(string name, string description, int accountId, IEnumerable<string> opportunityMembers);

        Task<IEnumerable<OpportunityListingServiceModel>> GetByUserIdAsync(string userId);
    }
}
