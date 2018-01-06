namespace OfferMaker.Web.Models.Proposal
{
    using Microsoft.AspNetCore.Http;
    using OfferMaker.Data;
    using System.ComponentModel.DataAnnotations;

    public class ProposalFormModel
    {
        [Required]
        [MinLength(DataConstants.EntityTitleMinLenght)]
        [MaxLength(DataConstants.EntityTitleMaxLenght)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Value { get; set; }

        [DisplayFormat(DataFormatString = "{0:P2}")]
        [Range(-100, 100)]
        public double Margin { get; set; }

        public int OpportunityId { get; set; }
        
        public IFormFile File { get; set; }
    }
}
