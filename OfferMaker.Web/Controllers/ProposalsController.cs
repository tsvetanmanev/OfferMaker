namespace OfferMaker.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Identity;
    using OfferMaker.Data.Models;
    using Microsoft.AspNetCore.Authorization;
    using OfferMaker.Services;
    using OfferMaker.Web.Models.Proposal;
    using OfferMaker.Web.Infrastructure.Extensions;
    using System.IO;
    using OfferMaker.Data;

    public class ProposalsController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly IProposalService proposals;
        private readonly IOpportunityService opportunities;
        private readonly IAccountService accounts;

        public ProposalsController(UserManager<User> userManager, IProposalService proposals, IOpportunityService opportunities, IAccountService accounts)
        {
            this.userManager = userManager;
            this.proposals = proposals;
            this.opportunities = opportunities;
            this.accounts = accounts;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var userId = this.userManager.GetUserId(User);

            return View(await this.proposals.GetAllByUserAsync(userId));
        }

        [Authorize(Roles = WebConstants.OpportunityMemberRole)]
        public async Task<IActionResult> Create(int opportunityId)
        {
            if (!await this.ValidateUserIsMemberOfOpportunityAsync(opportunityId))
            {
                return Unauthorized();
            }

            var model = new ProposalFormModel
            {
                OpportunityId = opportunityId
            };

            return View(model);
        }

        [Authorize(Roles = WebConstants.OpportunityMemberRole)]
        [HttpPost]
        public async Task<IActionResult> Create(ProposalFormModel model)
        {
            if (!await this.ValidateUserIsMemberOfOpportunityAsync(model.OpportunityId))
            {
                return Unauthorized();
            }

            if (model.File != null && (!model.File.FileName.EndsWith(".pdf") || model.File.Length > DataConstants.UploadFileLenght))
            {
                ModelState.AddModelError("File", "File should be .pdf, no larger than 5 MB in size!");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await this.proposals.CreateAsync(
                model.Name,
                model.Value,
                model.Margin,
                model.OpportunityId,
                model.File);

            TempData.AddSuccessMessage($"Proposal {model.Name} created successfully!");
            return RedirectToAction(controllerName: "Opportunities", actionName: nameof(OpportunitiesController.Details), routeValues: new { id = model.OpportunityId });
        }

        [Authorize]
        public async Task<IActionResult> DownloadFile(int proposalId)
        {
            var proposal = await this.proposals.GetByIdAsync(proposalId);
            if (proposal == null)
            {
                return BadRequest();
            }

            var userId = this.userManager.GetUserId(User);
            if (await this.ValidateUserIsMemberOfOpportunityAsync(proposal.OpportunityId) || await this.accounts.UserIsAssignedAccountManager(userId, proposal.AccountId))
            {
                var file = await this.proposals.GetFileAsync(proposalId);
                if (file == null)
                {
                    return BadRequest();
                }
                return File(file, "application/pdf", $"{proposal.Name} summary.pdf");
            }

            else
            {
                return Unauthorized();
            }            
        }

        private async Task<bool> ValidateUserIsMemberOfOpportunityAsync(int opportunityId)
        {
            var userId = this.userManager.GetUserId(User);

            var userIsMemberOfOpportunity = await this.opportunities.UserIsMemberOfOpportunity(userId, opportunityId);

            return userIsMemberOfOpportunity;
        }
    }
}