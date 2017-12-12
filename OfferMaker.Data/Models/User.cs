namespace OfferMaker.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using System.Collections.Generic;

    public class User : IdentityUser
    {
        public IEnumerable<UserOpportunity> Opportunities { get; set; }

        public IEnumerable<Account> Accounts { get; set; }
    }
}
