namespace OfferMaker.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using OfferMaker.Data.Models;
    using OfferMaker.Services;
    using OfferMaker.Web.Infrastructure.Extensions;
    using OfferMaker.Web.Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class OpportunitiesController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly IOpportunityService opportunities;

        public OpportunitiesController(UserManager<User> userManager, IOpportunityService opportunities)
        {
            this.userManager = userManager;
            this.opportunities = opportunities;
        }

        [Authorize]
        public async Task<IActionResult> Create(int accountId)
            => View(new AddOpportunityFormModel
            {
                AccountId = accountId,
                potentialMembers = await GetUsersInOpportunityMemberRole()
            });

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(AddOpportunityFormModel model)
        {
            if (!ModelState.IsValid)
            {
                model.potentialMembers = await GetUsersInOpportunityMemberRole();
                return View(model);
            }

            await this.opportunities.CreateAsync(
                model.Name,
                model.Description,
                model.AccountId,
                model.opportunityMembers
                );

            TempData.AddSuccessMessage($"Opportunity {model.Name} created successfully!");
            return RedirectToAction(controllerName: "Accounts", actionName: nameof(AccountsController.Details), routeValues: new { id = model.AccountId });
        }

        private async Task<IEnumerable<SelectListItem>> GetUsersInOpportunityMemberRole()
        {
            var opportunityMembers = await this.userManager.GetUsersInRoleAsync(WebConstants.OpportunityMemberRole);

            var opportunityMembersListItems = opportunityMembers
                .Select(t => new SelectListItem
                {
                    Text = t.UserName,
                    Value = t.Id
                });

            return opportunityMembersListItems;
        }
    }
}