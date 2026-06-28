using Microsoft.AspNetCore.Mvc;
using Pharmacy.Application.DTO.Contact;
using Pharmacy.Application.Services.Interfaces;
using Pharmacy.Web.PresentationExtensions;

namespace Pharmacy.Web.Controllers
{
    public class HomeController : SiteBaseController
    { 
        #region Field and ctor
        private readonly ISiteSettingService _siteSettingService;
        private readonly IContactService _contactService;   

        public HomeController(ISiteSettingService siteSettingService, IContactService contactService)
        {
            _siteSettingService = siteSettingService;
            _contactService = contactService;
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

       

        #region ContactUs
        [HttpGet("contact-us")]
        public async Task<IActionResult> ContactUs()
        {
            var setting = await _siteSettingService.GetDefaultSiteSetting();
            if (setting == null)
            {
                TempData[ErrorMessage] = "تنظیمات سایت یافت نشد";
                return RedirectToAction("Index", "Home");
            }

            ViewBag.SiteSetting = setting;
            return View();

        }
        [HttpPost("contact-us"), ValidateAntiForgeryToken]
        public async Task<IActionResult> ContactUs(CreateContactUsDto contact)
        {
         

            if (ModelState.IsValid)
            {
                var ip = HttpContext.GetUserIp();
                await _contactService.CreateContactUs(contact, ip, /*User.GetUserId()*/ null);
                TempData[SuccessMessage] = "پیام شما با موفقیت ارسال شد";
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
        #endregion

       

    }
}
