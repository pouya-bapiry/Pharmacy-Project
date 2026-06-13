using GoogleReCaptcha.V3.Interface;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Application.DTO.Account;
using Pharmacy.Application.Services.Interfaces;
using Pharmacy.Domain.Entities.Account;
using Pharmacy.Domain.IRepository;

namespace Pharmacy.Web.Controllers
{
    public class AccountController : SiteBaseController
    {
        #region Fields

        private readonly IUserService _userService;     
        private readonly IGenericRepository<User> _userRepository;       

        public AccountController(IUserService userService, IGenericRepository<User> userRepository)
        {
            _userService = userService;        
            _userRepository = userRepository;
          
        }


        #endregion

        #region Actions

        [HttpGet("register")]
        public async Task<IActionResult> RegisterUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }
            return View();
        }
        [HttpPost("register"), ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterUser(RegisterUserDto register)
        {
            //if (ModelState.IsValid)
            //{
            var result = await _userService.RegisterUser(register);
          

         

                switch (result)
                {
                    case RegisterUserResult.PasswordError:
                        TempData[InfoMessage] = $"رمز عبور وارد شده با رمز عبوری که قبلا ثبت کرده اید تطابقت ندارد";
                        TempData[WarningMessage] = $"لطفا رمز عبور را به درستی وارد کنید و همچنین حساب کاربری خود را فعال سازی کنید در صورت فراموشی رمز عبور با پشتیبانی تماس بگیرید";
                        return RedirectToAction("RegisterUser", "Account");                                    
                    case RegisterUserResult.MobileExists:
                        TempData[WarningMessage] = $"شماره همراه : {register.Mobile} تکراری می باشد.";
                        ModelState.AddModelError("Mobile", "شماره همراه تکراری می باشد.");
                    break;
                        return RedirectToAction("Index", "Home");
                    case RegisterUserResult.Error:
                        TempData[ErrorMessage] = "در ثبت اطلاعات خطایی رخ داد. لطفا دوباره تلاش نمایید.";
                        break;
                    case RegisterUserResult.Success:
                        TempData[SuccessMessage] = $"حساب کاربری شما با موفقیت ایجاد شد";                      
                        return RedirectToAction("Index", "Home");
                      

                }
            
            //}
            return View(register);
        }

        #endregion
    }
}
