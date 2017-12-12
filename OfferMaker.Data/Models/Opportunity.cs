namespace OfferMaker.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Opportunity
    {
        public int Id { get; set; }
        
        [Required]
        [MinLength(DataConstants.EntityTitleMinLenght)]
        [MaxLength(DataConstants.EntityTitleMaxLenght)]
        public string Name { get; set; }

        public DateTime CreationDate { get; set; }

        [MaxLength(DataConstants.EntitySummaryMaxLenght)]
        public string Description { get; set; }

        public int AccountId { get; set; }

        public Account Account { get; set; }

        public IEnumerable<Proposal> Proposals { get; set; }

        public IEnumerable<UserOpportunity> Members { get; set; }
    }
}
