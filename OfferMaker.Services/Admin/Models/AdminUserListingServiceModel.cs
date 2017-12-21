namespace OfferMaker.Services.Admin.Models
{
    using OfferMaker.Common.Mapping;
    using OfferMaker.Data.Models;

    public class AdminUserListingServiceModel : IMapFrom<User>
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }
    }
}
