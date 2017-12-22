namespace OfferMaker.Web.Models.Account
{
    using OfferMaker.Services.Models.Account;

    public class AccountDetailsViewModel
    {
        public AccountDetailsServiceModel Account { get; set; }

        public bool UserIsAssignedAccountManager { get; set; }
    }
}
