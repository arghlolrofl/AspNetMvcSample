using CrossNtErp.Web.Controllers.Base;
using CrossNtErp.Web.Helpers;
using CrossNtErp.Web.Security;
using System;
using System.Web;
using System.Web.Mvc;


namespace CrossNtErp.Web.Controllers {
    [Authorize]
    public class HomeController : LocalizationControllerBase {
        private AppIdentityContext db = new AppIdentityContext();

        public ActionResult Index() => View();

        public ActionResult About() => View();

        public ActionResult Contact() => View();

        [AllowAnonymous]
        public ActionResult SetCulture(string culture) {
            // Validate input
            culture = CultureHelper.GetImplementedCulture(culture);

            // Save culture in a cookie
            HttpCookie cookie = Request.Cookies[CultureCookieName];

            if (cookie != null)
                cookie.Value = culture;   // update cookie value
            else 
                cookie = new HttpCookie(CultureCookieName) { Value = culture, Expires = DateTime.Now.AddYears(1) };
            
            Response.Cookies.Add(cookie);

            return RedirectToAction("Index");
        }
    }
}