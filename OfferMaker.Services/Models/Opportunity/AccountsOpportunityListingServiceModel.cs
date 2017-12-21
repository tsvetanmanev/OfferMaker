namespace OfferMaker.Services.Models.Opportunity
{
    using OfferMaker.Common.Mapping;
    using OfferMaker.Data.Models;
    using System;

    public class AccountsOpportunityListingServiceModel : IMapFrom<Opportunity>
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public DateTime CreationDate { get; set; }
    }
}
