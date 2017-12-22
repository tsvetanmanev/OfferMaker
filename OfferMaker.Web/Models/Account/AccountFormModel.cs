namespace OfferMaker.Web.Models.Account
{
    using OfferMaker.Common.Mapping;
    using OfferMaker.Data;
    using OfferMaker.Services.Models.Account;
    using System.ComponentModel.DataAnnotations;

    public class AccountFormModel : IMapFrom<AccountDetailsServiceModel>
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
