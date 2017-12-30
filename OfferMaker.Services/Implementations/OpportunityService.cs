namespace OfferMaker.Services.Implementations
{
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;
    using OfferMaker.Data;
    using OfferMaker.Data.Models;
    using OfferMaker.Services.Models.Opportunity;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class OpportunityService : IOpportunityService
    {
        private readonly OfferMakerDbContext db;

        public OpportunityService(OfferMakerDbContext db)
        {
            this.db = db;
        }

        public async Task CreateAsync(string name, string description, int accountId, IEnumerable<string> opportunityMembers)
        {
            var creationDate = DateTime.UtcNow;

            var opportunity = new Opportunity
            {
                Name = name,
                Description = description,
                CreationDate = creationDate,
                AccountId = accountId
            };

            await this.db.AddAsync(opportunity);
            await this.db.SaveChangesAsync();

            if (opportunityMembers != null)
            {
                var opportunityId = opportunity.Id;
                foreach (var userId in opportunityMembers)
                {
                    var userInOpportunity = new UserOpportunity
                    {
                        OpportunityId = opportunityId,
                        UserId = userId
                    };

                    this.db.Add(userInOpportunity);
                }

                await this.db.SaveChangesAsync();
            }

        }

        public async Task<bool> DeleteAsync(int id)
        {
            var opportunity = await this.db.Opportunities.FindAsync(id);

            if (opportunity == null)
            {
                return false;
            }

            this.db.Opportunities.Remove(opportunity);
            await this.db.SaveChangesAsync();

            return true;
        }

        public async Task EditAsync(int id, string name, string description, IEnumerable<string> opportunityMembers)
        {
            var opportunity = await this.db.Opportunities.FirstOrDefaultAsync(o => o.Id == id);

            if (opportunity == null)
            {
                return;
            }

            opportunity.Name = name;
            opportunity.Description = description;

            await this.db.SaveChangesAsync();


            //TODO: Fix logic below for updating Opportunity Members
            if (opportunityMembers != null)
            {
                var opportunityId = opportunity.Id;
                foreach (var userId in opportunityMembers)
                {
                    var userInOpportunity = new UserOpportunity
                    {
                        OpportunityId = opportunityId,
                        UserId = userId
                    };

                    this.db.Add(userInOpportunity);
                }

                await this.db.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<OpportunityListingServiceModel>> GetAllByUserIdAsync(string userId)
            => await this.db
                    .Opportunities
                    .Where(o => o.Members.Any(m => m.UserId == userId))
                    .ProjectTo<OpportunityListingServiceModel>()
                    .ToListAsync();

        public async Task<OpportunityDetailsServiceModel> GetByIdAsync(int id)
            => await this.db
                    .Opportunities
                    .Where(o => o.Id == id)
                    .ProjectTo<OpportunityDetailsServiceModel>()
                    .FirstOrDefaultAsync();

        public async Task<bool> UserIsMemberOfOpportunity(string userId, int opportunityId)
            => await this.db
                .Opportunities
                .AnyAsync(o => o.Id == opportunityId && o.Members.Any(uo => uo.UserId == userId));
    }
}
