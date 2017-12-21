namespace OfferMaker.Web.Areas.Admin.Models
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using OfferMaker.Services.Admin.Models;
    using System.Collections.Generic;

    public class UserListingViewModel
    {
        public IEnumerable<SelectListItem> Roles { get; set; }

        public IEnumerable<AdminUserListingServiceModel> Users { get; set; }
    }
}
