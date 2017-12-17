namespace OfferMaker.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using OfferMaker.Services;
    using System.Threading.Tasks;

    public class AccountsController : Controller
    {
        private readonly IAccountService accounts;

        public AccountsController(IAccountService accounts)
        {
            this.accounts = accounts;
        }

        public async Task<IActionResult> Index()
        {
            return View(await this.accounts.GetAllAsync());
        }
    }
}