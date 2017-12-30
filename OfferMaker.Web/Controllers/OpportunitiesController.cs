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
    using AutoMapper;
    using OfferMaker.Services.Models.Opportunity;

    public class OpportunitiesController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly IOpportunityService opportunities;
        private readonly IAccountService accounts;

        public OpportunitiesController(UserManager<User> userManager, IOpportunityService opportunities, IAccountService accounts)
        {
            this.userManager = userManager;
            this.opportunities = opportunities;
            this.accounts = accounts;
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
            if (!await this.ValidateUserIsAssignedAccountManager(accountId))
            {
                return Unauthorized();
            }

            var model = new OpportunityFormModel
            {
                AccountId = accountId,
                PotentialMembers = await GetUsersInOpportunityMemberRoleAsync()
            };

            return View(model);
        }

        [Authorize(Roles = WebConstants.AccountManagerRole)]
        [HttpPost]
        public async Task<IActionResult> Create(OpportunityFormModel model)
        {
            if (!await this.ValidateUserIsAssignedAccountManager(model.AccountId))
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                model.PotentialMembers = await GetUsersInOpportunityMemberRoleAsync();
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
            if (!await this.ValidateUserIsMemberOfOpportunityAsync(id))
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
            if (!await this.ValidateUserIsMemberOfOpportunityAsync(id))
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

        [Authorize(Roles = WebConstants.OpportunityMemberRole)]
        public async Task<IActionResult> Edit(int id)
        {
            if (!await this.ValidateUserIsMemberOfOpportunityAsync(id))
            {
                return Unauthorized();
            }

            var serviceModel = await this.opportunities.GetByIdAsync(id);

            if (serviceModel == null)
            {
                return BadRequest();
            }

            var viewModel = Mapper.Map<OpportunityDetailsServiceModel, OpportunityFormModel>(serviceModel);

            viewModel.PotentialMembers = await this.GetUsersInOpportunityMemberRoleAsync();

            return this.ViewOrNotFound(viewModel);
        }

        [Authorize(Roles = WebConstants.OpportunityMemberRole)]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, OpportunityFormModel model)
        {
            if (!await this.ValidateUserIsAssignedAccountManager(model.AccountId))
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                model.PotentialMembers = await GetUsersInOpportunityMemberRoleAsync();
                return View(model);
            }

            var serviceModel = await this.opportunities.GetByIdAsync(id);

            if (serviceModel == null)
            {
                return BadRequest();
            }

            await this.opportunities.EditAsync(
                id,
                model.Name,
                model.Description,
                model.OportunityMembers);

            TempData.AddSuccessMessage($"Opportunity {model.Name} edited successfully!");
            return RedirectToAction(nameof(Details), routeValues: new { id = id });
        }

        private async Task<bool> ValidateUserIsMemberOfOpportunityAsync(int opportunityId)
        {
            var userId = this.userManager.GetUserId(User);

            var userIsMemberOfOpportunity = await this.opportunities.UserIsMemberOfOpportunity(userId, opportunityId);

            return userIsMemberOfOpportunity;
        }

        private async Task<IEnumerable<SelectListItem>> GetUsersInOpportunityMemberRoleAsync()
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

        private async Task<bool> ValidateUserIsAssignedAccountManager(int accountid)
        {
            var userId = this.userManager.GetUserId(User);
            var userIsAssignedAccountManager = await this.accounts.UserIsAssignedAccountManager(userId, accountid);

            return userIsAssignedAccountManager;
        }
    }
}