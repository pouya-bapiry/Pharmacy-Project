using Microsoft.AspNetCore.Mvc;
using Pharmacy.Web.Models;
using System.Diagnostics;

namespace Pharmacy.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    
    }
}
