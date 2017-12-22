namespace OfferMaker.Web.Models.Account
{
    using OfferMaker.Data;
    using System.ComponentModel.DataAnnotations;

    public class AddAccountFormModel
    {
        [Required]
        [MinLength(DataConstants.EntityTitleMinLenght)]
        [MaxLength(DataConstants.EntityTitleMaxLenght)]
        public string Name { get; set; }

        public string Address { get; set; }

        [Required]
        [MaxLength(DataConstants.DescriptionMaxLenght)]
        public string Description { get; set; }
    }
}
