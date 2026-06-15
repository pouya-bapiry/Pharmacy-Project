using Microsoft.AspNetCore.Mvc;
using Pharmacy.Application.Services.Interfaces;
using Pharmacy.Web.Models;
using System.Diagnostics;

namespace Pharmacy.Web.Controllers
{
    public class HomeController : Controller
    { 
        #region Field and ctor
        private readonly ISiteSettingService _siteSettingService;

        public HomeController(ISiteSettingService siteSettingService)
        {
            _siteSettingService = siteSettingService;
        }
        #endregion

        public IActionResult Index()
        {
            return View();
        }

        #region AboutUs
        [HttpGet("about-us")]
        public async Task<IActionResult> AboutUs()
        {
            var about = await _siteSettingService.GetAboutUs();
            return View(about);
        }
        #endregion

    }
}
