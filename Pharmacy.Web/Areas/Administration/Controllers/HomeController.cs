using Microsoft.AspNetCore.Mvc;
using Pharmacy.Application.DTO.Site;
using Pharmacy.Application.Services.Interfaces;
using Pharmacy.Web.PresentationExtensions;

namespace Pharmacy.Web.Areas.Administration.Controllers
{
    public class HomeController : AdminBaseController
    {

        #region Fields and ctor 

        private readonly IUserService _userService;
        private readonly ISiteSettingService _siteSettingService;

        public HomeController(IUserService userService, ISiteSettingService siteSettingService)
        {

            _userService = userService;
            _siteSettingService = siteSettingService;
        }


        #endregion
        public IActionResult Index()
        {
            return View();
        }

        #region Site Setting

        #region Get
        [HttpGet("site-setting")]
        public async Task<IActionResult> SiteSetting()
        {
            var setting = await _siteSettingService.GetDefaultSiteSetting();
            return View(setting);
        }
        #endregion



        #region Edit Site Setting

        [HttpGet("edit-sitesetting/{SettingId}")]
        public async Task<IActionResult> EditSiteSetting(long id)
        {
            var edit = await _siteSettingService.GetSiteSettingForEdit(id);

            return View(edit);

        }
        [HttpPost("edit-sitesetting/{SettingId}")]
        public async Task<IActionResult> EditSiteSetting(EditSiteSettingDto edit)
        {
            var user = await _userService.GetUserById(User.GetUserId());
            var username = user.FirstName + " " + user.LastName;
            var result = await _siteSettingService.EditSiteSetting(edit, username);

            switch (result)
            {
                case true:
                    return RedirectToAction("SiteSetting", "Home");
                case false:
                    return null;


            }
            return View(edit);
        }
        #endregion

        #endregion

        #region GetAllAboutUs

        [HttpGet("about-us")]
        //public async Task<IActionResult> AboutUsList()
        //{
        //    var about = await _contactService.GetAll();
        //    return View(about);
        //}

        #endregion

        #region Create AboutUS

        [HttpGet("create-about-us")]
        public async Task<IActionResult> CreateAboutUs()
        {
            return View();
        }

        //[HttpPost("create-about-us"), ValidateAntiForgeryToken]
        //public async Task<IActionResult> CreateAboutUs(CreateAboutUsDto create)
        //{
        //    var result = await _contactService.CreateAboutUs(create);

        //    switch (result)
        //    {
        //        case CreateAboutUsResult.Error:
        //            TempData[ErrorMessage] = "در افزودن اطلاعات خطایی رخ داد";
        //            break;
        //        case CreateAboutUsResult.Success:
        //            TempData[SuccessMessage] = "عملیات ثبت اطلاعات با موفقیت انجام شد";
        //            return RedirectToAction("AboutUsList", "Home");
        //    }

        //    return View();
        //}
        #endregion

        #region Edit AboutUs
        #endregion
    }
}

