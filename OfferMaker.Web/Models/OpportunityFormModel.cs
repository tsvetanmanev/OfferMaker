namespace OfferMaker.Web.Models
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using OfferMaker.Common.Mapping;
    using OfferMaker.Data;
    using OfferMaker.Services.Models.Opportunity;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class OpportunityFormModel : IMapFrom<OpportunityDetailsServiceModel>
    {
        [Required]
        [MinLength(DataConstants.EntityTitleMinLenght)]
        [MaxLength(DataConstants.EntityTitleMaxLenght)]
        public string Name { get; set; }

        [MaxLength(DataConstants.EntitySummaryMaxLenght)]
        public string Description { get; set; }

        public int AccountId { get; set; }

        public IEnumerable<SelectListItem> PotentialMembers { get; set; }

        public IEnumerable<string> OportunityMembers { get; set; }
    }
}
