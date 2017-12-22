namespace OfferMaker.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using OfferMaker.Data.Models;
    using OfferMaker.Services;
    using OfferMaker.Web.Infrastructure.Extensions;
    using OfferMaker.Web.Models;
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

        [Authorize]
        public IActionResult Create()
            => View();

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(AddAccountFormModel accountModel)
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
            var model = await this.accounts.GetByIdAsync(id);
            return this.ViewOrNotFound(model);
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var model = await this.accounts.GetByIdAsync(id);
            return this.ViewOrNotFound(model);
        }

        [Authorize]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var model = await this.accounts.GetByIdAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            var result = await this.accounts.DeleteAsync(id);

            if (result)
            {
                TempData.AddSuccessMessage($"Account {model.Name} deleted successfully!");
                return RedirectToAction(nameof(Index));
            }

            return NotFound();
        }
    }
}