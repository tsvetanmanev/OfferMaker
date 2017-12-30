namespace OfferMaker.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using OfferMaker.Web.Models;
    using System.Diagnostics;

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Redirect("/");
        }
        

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
