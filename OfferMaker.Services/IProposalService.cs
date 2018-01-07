namespace OfferMaker.Services
{
    using Microsoft.AspNetCore.Http;
    using OfferMaker.Services.Models.Proposal;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IProposalService
    {
        Task<IEnumerable<ProposalListingServiceModel>> GetAllByUserAsync(string userId);

        Task<IEnumerable<ProposalListingServiceModel>> GetAllByManagerAsync(string userId);

        Task CreateAsync(string name, decimal value, double margin, int opportunityId, IFormFile file);

        Task<byte[]> GetFileAsync(int proposalId);

        Task<ProposalServiceModel> GetByIdAsync(int proposalId);

        Task<bool> ApproveAsync(int proposalId);

        Task<bool> RejectAsync(int proposalId);
    }
}
