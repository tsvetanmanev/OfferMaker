namespace OfferMaker.Services.Implementations
{
    using AutoMapper.QueryableExtensions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using OfferMaker.Data;
    using OfferMaker.Data.Models;
    using OfferMaker.Services.Models;
    using OfferMaker.Services.Models.Proposal;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ProposalService : IProposalService
    {
        private readonly OfferMakerDbContext db;
        private readonly IGoogleDriveService googleDriveService;

        public ProposalService(OfferMakerDbContext db, IGoogleDriveService googleDriveService)
        {
            this.db = db;
            this.googleDriveService = googleDriveService;
        }

        public async Task CreateAsync(string name, decimal value, double margin, int opportunityId, IFormFile formFile)
        {
            string fileId = null;
            if (formFile != null)
            {
                 fileId = await googleDriveService.UploadFileAsync(formFile);
            }

            var proposal = new Proposal
            {
                Name = name,
                Date = DateTime.UtcNow,
                Value = value,
                Margin = margin * (0.01),
                FileId = fileId,
                OpportunityId = opportunityId
            };

            await this.db.AddAsync(proposal);
            await this.db.SaveChangesAsync();
        }
        
        public async Task<IEnumerable<ProposalListingServiceModel>> GetAllByUserAsync(string userId)
            => await this.db
                        .Proposals
                        .Where(p => p.Opportunity.Members.Any(uo => uo.UserId == userId))
                        .ProjectTo<ProposalListingServiceModel>()
                        .ToListAsync();


        public async Task<IEnumerable<ProposalListingServiceModel>> GetAllByManagerAsync(string userId)
            => await this.db
                        .Proposals
                        .Where(p => p.Opportunity.Account.ManagerId == userId)
                        .ProjectTo<ProposalListingServiceModel>()
                        .ToListAsync();

        public async Task<ProposalServiceModel> GetByIdAsync(int proposalId)
        => await this.db
                    .Proposals
                    .Where(p => p.Id == proposalId)
                    .ProjectTo<ProposalServiceModel>()
                    .FirstOrDefaultAsync();

        public async Task<byte[]> GetFileAsync(int proposalId)
        {
            var proposal = await this.db.FindAsync<Proposal>(proposalId);

            if (proposal == null)
            {
                return null;
            }

            var fileId = proposal.FileId;
            if (fileId == null)
            {
                return null;
            }

            var file = await googleDriveService.DownloadFileAsync(fileId);

            return file;
        }

        public async Task<bool> ApproveAsync(int proposalId)
        {
            var proposal = await this.db.FindAsync<Proposal>(proposalId);

            if (proposal == null)
            {
                return false;
            }

            if (proposal.Status != ApprovalStatus.Pending)
            {
                return false;
            }

            proposal.Status = ApprovalStatus.Approved;

            await this.db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RejectAsync(int proposalId)
        {
            var proposal = await this.db.FindAsync<Proposal>(proposalId);

            if (proposal == null)
            {
                return false;
            }

            if (proposal.Status != ApprovalStatus.Pending)
            {
                return false;
            }

            proposal.Status = ApprovalStatus.Rejected;

            await this.db.SaveChangesAsync();

            return true;
        }
    }
}
