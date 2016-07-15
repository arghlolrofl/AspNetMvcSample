using CrossNtErp.Web.Controllers.Base;
using System.Web.Mvc;

namespace CrossNtErp.Web.Controllers {
    public class ErrorsController : LocalizationControllerBase {
        // GET: Errors/UnAuthorized
        public ActionResult UnAuthorized() {
            return View();
        }
    }
}