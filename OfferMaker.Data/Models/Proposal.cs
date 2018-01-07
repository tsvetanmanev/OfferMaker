namespace OfferMaker.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Proposal
    {
        public int Id { get; set; }

        [Required]
        [MinLength(DataConstants.EntityTitleMinLenght)]
        [MaxLength(DataConstants.EntityTitleMaxLenght)]
        public string Name { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public decimal Value { get; set; }

        public double Margin { get; set; }

        public string FileId { get; set; }

        public int OpportunityId { get; set; }

        public Opportunity Opportunity { get; set; }

        [Required]
        public ApprovalStatus Status { get; set; }
    }
}
