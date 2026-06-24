using Microsoft.AspNetCore.Mvc;

namespace Pharmacy.Web.Areas.Administration.Controllers
{
    public class HomeController : AdminBaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
