namespace OfferMaker.Services.Models.Proposal
{
    using OfferMaker.Common.Mapping;
    using OfferMaker.Data.Models;
    using System;
    using AutoMapper;

    public class ProposalListingServiceModel: IMapFrom<Proposal>, IHaveCustomMapping
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime Date { get; set; }

        public decimal Value { get; set; }

        public double Margin { get; set; }

        public ApprovalStatus Status { get; set; }

        public bool FileExists { get; set; }

        public void ConfigureMapping(Profile mapper)
        {
            mapper.CreateMap<Proposal, ProposalListingServiceModel>()
                .ForMember(p => p.FileExists, cfg => cfg.MapFrom(p => p.FileId != null));
        }
    }
}
