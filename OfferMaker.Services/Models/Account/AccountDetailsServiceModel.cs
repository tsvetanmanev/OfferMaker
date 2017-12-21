namespace OfferMaker.Services.Models.Account
{
    using OfferMaker.Common.Mapping;
    using OfferMaker.Data.Models;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using OfferMaker.Services.Models.Opportunity;

    public class AccountDetailsServiceModel : IMapFrom<Account>, IHaveCustomMapping
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Description { get; set; }

        public string ManagerName { get; set; }

        public IEnumerable<AccountsOpportunityListingServiceModel> Oppotunities { get; set; }

        public void ConfigureMapping(Profile mapper)
            => mapper
                .CreateMap<Account, AccountDetailsServiceModel>()
                .ForMember(ad => ad.ManagerName, cfg => cfg.MapFrom(a => a.Manager.FirstName + " " + a.Manager.LastName))
                .ForMember(ad => ad.Oppotunities, cfg => cfg.MapFrom(a => a.Opportunities));
    }
}
