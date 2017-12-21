using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OfferMaker.Services
{
    public interface IOpportunityService
    {
        Task CreateAsync(string name, string description, int accountId, IEnumerable<string> opportunityMembers);
    }
}
