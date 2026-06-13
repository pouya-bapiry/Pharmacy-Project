using Microsoft.AspNetCore.Mvc;

namespace Pharmacy.Web.Controllers
{
    public class SiteBaseController : Controller
    {
        protected String ErrorMessage = "ErrorMessage";
        protected String SuccessMessage = "SuccessMessage";
        protected String InfoMessage = "InfoMessage";
        protected String WarningMessage = "WarningMessage";

    }
}
