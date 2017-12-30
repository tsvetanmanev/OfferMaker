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
    using System;

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
        public async Task<IActionResult> Index()
        {
            var userId = this.userManager.GetUserId(User);

            return View(await this.opportunities.GetAllByUserIdAsync(userId));
        }

        [Authorize]
        public async Task<IActionResult> Create(int accountId)
        {
            var model = new AddOpportunityFormModel
            {
                AccountId = accountId,
                PotentialMembers = await GetUsersInOpportunityMemberRole()
            };

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(AddOpportunityFormModel model)
        {
            if (!ModelState.IsValid)
            {
                model.PotentialMembers = await GetUsersInOpportunityMemberRole();
                return View(model);
            }

            await this.opportunities.CreateAsync(
                model.Name,
                model.Description,
                model.AccountId,
                model.OportunityMembers
                );

            TempData.AddSuccessMessage($"Opportunity {model.Name} created successfully!");
            return RedirectToAction(controllerName: "Accounts", actionName: nameof(AccountsController.Details), routeValues: new { id = model.AccountId });
        }

        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var model = await this.opportunities.GetByIdAsync(id);

            return this.ViewOrNotFound(model);
        }

        [Authorize(Roles = WebConstants.OpportunityMemberRole)]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await this.ValidateUserIsMemberOfOpportunity(id))
            {
                return Unauthorized();
            }

            var model = await this.opportunities.GetByIdAsync(id);

            return this.ViewOrNotFound(model);
        }

        [Authorize(Roles = WebConstants.OpportunityMemberRole)]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!await this.ValidateUserIsMemberOfOpportunity(id))
            {
                return Unauthorized();
            }

            var model = await this.opportunities.GetByIdAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            var result = await this.opportunities.DeleteAsync(id);
            if (!result)
            {
                return NotFound();
            }

            TempData.AddSuccessMessage($"Opportunity {model.Name} deleted successfully!");
            return RedirectToAction("Details", "Accounts", new { id = model.AccountId });
        }

        private async Task<bool> ValidateUserIsMemberOfOpportunity(int opportunityId)
        {
            var userId = this.userManager.GetUserId(User);

            var userIsMemberOfOpportunity = await this.opportunities.UserIsMemberOfOpportunity(userId, opportunityId);

            return userIsMemberOfOpportunity;
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