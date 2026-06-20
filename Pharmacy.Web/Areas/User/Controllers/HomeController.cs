using Microsoft.AspNetCore.Mvc;

namespace Pharmacy.Web.Areas.User.Controllers
{
    public class HomeController : UserBaseController
    {

        #region User Dashboard

        [HttpGet("user-dashboard")]
        public async Task<IActionResult> Dashboard()
        {
            return View();
        }

        #endregion
    }
}
