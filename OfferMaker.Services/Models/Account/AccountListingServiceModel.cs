namespace OfferMaker.Services.Models.Account
{
    using AutoMapper;
    using OfferMaker.Common.Mapping;
    using OfferMaker.Data.Models;
    using System.Linq;

    public class AccountListingServiceModel : IMapFrom<Account>, IHaveCustomMapping
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Description { get; set; }

        public int OpportunitiesCount { get; set; }

        public string ManagerName { get; set; }

        public void ConfigureMapping(Profile mapper)
            => mapper
                .CreateMap<Account, AccountListingServiceModel>()
                .ForMember(a => a.OpportunitiesCount, cfg => cfg.MapFrom(a => a.Opportunities.Count()))
                .ForMember(a => a.ManagerName, cfg => cfg.MapFrom(a => a.Manager.FirstName + " " + a.Manager.LastName));

    }
}
