namespace OfferMaker.Services.Models
{
    using OfferMaker.Common.Mapping;
    using OfferMaker.Data.Models;

    public class UserListingServiceModel : IMapFrom<User>
    {
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
    }
}
