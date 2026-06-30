using Microsoft.AspNetCore.Mvc;
using Pharmacy.Application.DTO.Site.Slider;
using Pharmacy.Application.Services.Implementation;
using Pharmacy.Application.Services.Interfaces;
using Pharmacy.Domain.IRepository;
using Pharmacy.Web.PresentationExtensions;

namespace Pharmacy.Web.Areas.Administration.Controllers
{
    public class ImageController : AdminBaseController
    {
        #region Fields and Ctor
        private readonly ISiteImagesService _siteImagesService;
        private readonly IUserService _userService;

        public ImageController(ISiteImagesService siteImagesService,IUserService userService)
        {
            _siteImagesService = siteImagesService;
            _userService = userService;
        }



        #endregion

        #region Slider Actions

        #region Get
        [HttpGet("slider-list")]
        public async Task<IActionResult> SliderList()
        {
            var sliders = await _siteImagesService.GetAllSlider();
            return View(sliders);
        }

        #endregion

        #region Create
        [HttpGet("create-slider")]
        public async Task<IActionResult> CreateSlider()
        {
            return View();
        }

        [HttpPost("create-slider")]
        public async Task<IActionResult> CreateSlider(CreateSliderDto slider, IFormFile sliderImage,
            IFormFile mobileSliderImage)
        {
            
            var result = await _siteImagesService.CreateSlider(slider, sliderImage, mobileSliderImage);

            switch (result)
            {
                case CreateSliderResult.Error:
                    TempData[ErrorMessage] = "در افزودن اسلایدر خطایی رخ داد";
                    break;
                case CreateSliderResult.Success:
                    TempData[SuccessMessage] = "عملیات ثبت اسلایدر با موفقیت انجام شد";
                    return RedirectToAction("SliderList", "Image");
            }

            return View();
        }

        #endregion

        #region Edit

        [HttpGet("edit-slider/{sliderId}")]
        public async Task<IActionResult> EditSlider(long sliderId)
        {
            var slider = await _siteImagesService.GetSliderForEdit(sliderId);
            return View(slider);
        }

        [HttpPost("edit-slider/{sliderId}")]
        public async Task<IActionResult> EditSlider(EditSliderDto edit, IFormFile sliderImage,
            IFormFile mobileSliderImage)
        {
            if (ModelState.IsValid || edit.ImageName == null || edit.MobileImageName == null)
            {
                var user = await _userService.GetUserById(User.GetUserId());
                var username = user.FirstName + " " + user.LastName;
                var result = await _siteImagesService.EditSlider(edit, sliderImage, mobileSliderImage, username);
                switch (result)
                {
                    case EditSliderResult.Error:
                        TempData[ErrorMessage] = "در ویرایش اسلایدر خطایی رخ داد";
                        break;
                    case EditSliderResult.NotFound:
                        TempData[WarningMessage] = "اسلایدر مورد نظر یافت نشد";
                        break;
                    case EditSliderResult.Success:
                        TempData[SuccessMessage] = "ویرایش اسلایدر با موفقیت انجام شد";
                        return RedirectToAction("SliderList", "Image");
                }
            }




            return View();
        }


        #endregion

        #region Active DeActive

        [HttpGet("active-slider/{sliderId}")]
        public async Task<IActionResult> ActiveSlider(long sliderId)
        {
            var user = await _userService.GetUserById(User.GetUserId());
            var username = user.FirstName + " " + user.LastName;
            var result = await _siteImagesService.ActiveSlider(sliderId, username);
            if (result)
            {
                TempData[SuccessMessage] = "اسلایدر با موفقیت فعال شد";
                return RedirectToAction("SliderList", "Image", new { area = "Administration" });
            }

            TempData[ErrorMessage] = "خطا در فعال سازی اسلایدر";
            return RedirectToAction("SliderList", "Image", new { area = "Administration" });
        }

        [HttpGet("deactive-slider/{sliderId}")]
        public async Task<IActionResult> DeactiveSlider(long sliderId)
        {
            var user = await _userService.GetUserById(User.GetUserId());
            var username = user.FirstName + " " + user.LastName;
            var result = await _siteImagesService.DeActiveSlider(sliderId, username);
            if (result)
            {
                TempData[SuccessMessage] = "اسلایدر با موفقیت فعال شد";
                return RedirectToAction("SliderList", "Image", new { area = "Administration" });
            }

            TempData[ErrorMessage] = "خطا در فعال سازی اسلایدر";
            return RedirectToAction("SliderList", "Image", new { area = "Administration" });


        }

        #endregion


        #endregion
    }
}
