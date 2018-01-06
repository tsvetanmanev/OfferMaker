namespace OfferMaker.Services.Models.Opportunity
{
    using OfferMaker.Common.Mapping;
    using OfferMaker.Data.Models;
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using AutoMapper;
    using OfferMaker.Services.Models.Proposal;

    public class OpportunityDetailsServiceModel : IMapFrom<Opportunity>, IHaveCustomMapping
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public DateTime CreationDate { get; set; }
        
        public string Description { get; set; }

        public int AccountId { get; set; }

        public string AccountName { get; set; }

        public IEnumerable<ProposalListingServiceModel> Proposals { get; set; }

        public IEnumerable<UserListingServiceModel> Members { get; set; }

        public void ConfigureMapping(Profile mapper)
            => mapper
                .CreateMap<Opportunity, OpportunityDetailsServiceModel>()
                .ForMember(od => od.AccountName, cfg => cfg.MapFrom(o => o.Account.Name))
                .ForMember(od => od.Proposals, cfg => cfg.MapFrom(o => o.Proposals))
                .ForMember(od => od.Members, cfg => cfg.MapFrom(o => o.Members.Select(m => m.User)));
                

    }
}
