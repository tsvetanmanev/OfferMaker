namespace OfferMaker.Services.Models.Proposal
{
    using AutoMapper;
    using OfferMaker.Common.Mapping;
    using OfferMaker.Data.Models;

    public class ProposalServiceModel : IMapFrom<Proposal>, IHaveCustomMapping
    {
        public string Name { get; set; }

        public int OpportunityId { get; set; }

        public int AccountId { get; set; }

        public void ConfigureMapping(Profile mapper)
        {
            mapper.CreateMap<Proposal, ProposalServiceModel>()
                .ForMember(p => p.AccountId, cfg => cfg.MapFrom(p => p.Opportunity.AccountId));
        }
    }
}
