namespace OfferMaker.Web.Models
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using OfferMaker.Data;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class AddOpportunityFormModel
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
