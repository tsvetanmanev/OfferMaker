namespace OfferMaker.Services.Models
{
    using OfferMaker.Common.Mapping;
    using OfferMaker.Data.Models;
    using System;

    public class ProposalListingServiceModel: IMapFrom<Proposal>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime Date { get; set; }

        public decimal Value { get; set; }
    }
}
