namespace OfferMaker.Services.Models.Opportunity
{
    using OfferMaker.Common.Mapping;
    using OfferMaker.Data.Models;
    using System;
    using AutoMapper;

    public class OpportunityListingServiceModel : IMapFrom<Opportunity>, IHaveCustomMapping
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime CreationDate { get; set; }

        public int AccountId { get; set; }

        public string AccountName { get; set; }

        public void ConfigureMapping(Profile mapper)
            => mapper
                .CreateMap<Opportunity, OpportunityListingServiceModel>()
                .ForMember(mo => mo.AccountName, cfg => cfg.MapFrom(a => a.Account.Name));
    }
}
