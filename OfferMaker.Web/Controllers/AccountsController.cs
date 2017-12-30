namespace OfferMaker.Web.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using OfferMaker.Data.Models;
    using OfferMaker.Services;
    using OfferMaker.Services.Models.Account;
    using OfferMaker.Web.Infrastructure.Extensions;
    using OfferMaker.Web.Models;
    using OfferMaker.Web.Models.Account;
    using System.Threading.Tasks;


    public class AccountsController : Controller
    {
        private readonly IAccountService accounts;
        private readonly UserManager<User> userManager;

        public AccountsController(IAccountService accounts, UserManager<User> userManager)
        {
            this.accounts = accounts;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            return View(await this.accounts.GetAllAsync());
        }

        [Authorize(Roles = WebConstants.AccountManagerRole)]
        public IActionResult Create()
            => View();

        [Authorize(Roles = WebConstants.AccountManagerRole)]
        [HttpPost]
        public async Task<IActionResult> Create(AccountFormModel accountModel)
        {
            if (!ModelState.IsValid)
            {
                return View(accountModel);
            }

            var userId = this.userManager.GetUserId(User);

            await this.accounts.CreateAsync(
                accountModel.Name,
                accountModel.Address,
                accountModel.Description,
                userId);

            TempData.AddSuccessMessage($"Account {accountModel.Name} created successfully!");
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var account = await this.accounts.GetByIdAsync(id);

            if (account == null)
            {
                return NotFound();
            }

            var userId = this.userManager.GetUserId(User);
            var userIsAssignedAccountManager = await this.accounts.UserIsAssignedAccountManager(userId, id);

            var accountViewModel = new AccountDetailsViewModel
            {
                Account = account,
                UserIsAssignedAccountManager = userIsAssignedAccountManager
            };

            return View(accountViewModel);
        }

        [Authorize(Roles = WebConstants.AccountManagerRole)]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await this.ValidateUserIsAssignedAccountManager(id))
            {
                return Unauthorized();
            }

            var model = await this.accounts.GetByIdAsync(id);
            return this.ViewOrNotFound(model);
        }

        [Authorize(Roles = WebConstants.AccountManagerRole)]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!await this.ValidateUserIsAssignedAccountManager(id))
            {
                return Unauthorized();
            }

            var model = await this.accounts.GetByIdAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            var result = await this.accounts.DeleteAsync(id);
            if (!result)
            {
                return NotFound();
            }

            TempData.AddSuccessMessage($"Account {model.Name} deleted successfully!");
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = WebConstants.AccountManagerRole)]
        public async Task<IActionResult> Edit(int id)
        {
            if (!await this.ValidateUserIsAssignedAccountManager(id))
            {
                return Unauthorized();
            }

            var serviceModel = await this.accounts.GetByIdAsync(id);

            var viewModel = Mapper.Map<AccountDetailsServiceModel, AccountFormModel>(serviceModel);

            return this.ViewOrNotFound(viewModel);
        }

        [Authorize(Roles = WebConstants.AccountManagerRole)]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, AccountFormModel model)
        {
            if (!await this.ValidateUserIsAssignedAccountManager(id))
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var serviceModel = await this.accounts.GetByIdAsync(id);

            if (serviceModel == null)
            {
                return BadRequest();
            }

            await this.accounts.EditAsync(id, model.Name, model.Description, model.Address);

            TempData.AddSuccessMessage($"Account {model.Name} edited successfully!");
            return RedirectToAction(nameof(Details), routeValues: new { id = id });
        }

        private async Task<bool> ValidateUserIsAssignedAccountManager(int accountid)
        {
            var userId = this.userManager.GetUserId(User);
            var userIsAssignedAccountManager = await this.accounts.UserIsAssignedAccountManager(userId, accountid);

            return userIsAssignedAccountManager;
        }
    }
}