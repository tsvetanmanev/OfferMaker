namespace OfferMaker.Services.Implementations
{
    using OfferMaker.Data;
    using OfferMaker.Data.Models;
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
    }
}
