using CrossNtErp.Web.Helpers;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace CrossNtErp.Web.Controllers.Base {
    public class LocalizationControllerBase : Controller {
        protected const string CultureCookieName = "_locale";

        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state) {
            string cultureName = null;

            // Attempt to read the culture cookie from Request
            HttpCookie cultureCookie = Request.Cookies[CultureCookieName];
            if (cultureCookie != null)
                cultureName = cultureCookie.Value;
            else
                cultureName = Request.UserLanguages != null && Request.UserLanguages.Length > 0 ?
                        Request.UserLanguages[0] :  // obtain it from HTTP header AcceptLanguages
                        null;

            // Validate culture name
            cultureName = CultureHelper.GetImplementedCulture(cultureName); // This is safe

            ViewBag.Language = cultureName;

            // Modify current thread's cultures            
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            return base.BeginExecuteCore(callback, state);
        }

        protected override void OnException(ExceptionContext filterContext) {
            Exception ex = filterContext.Exception;
            do {
                LogException(ex);
            } while ((ex = ex.InnerException) != null);

            base.OnException(filterContext);
        }

        private void LogException(Exception ex, bool includeStackTrace = true) {
            Log("");
            Log($"ERROR [{DateTime.Now.ToLongTimeString()}]: {ex.GetType().Name}");

            string[] lines = ex.Message.Split(':');
            for (int i = 0; i < lines.Length - 1; i++)
                Log($"    {lines[i]}");

            lines = lines[lines.Length - 1].Split('.');
            foreach (string line in lines)
                Log($"    {line}.");

            if (ex.GetType() == typeof(DataException)) {
                var dex = ex as DataException;
                if (dex != null) {
                    Log($"ENTITY VALIDATION ERROR");
                    var eex = dex.InnerException as System.Data.Entity.Validation.DbEntityValidationException;
                    if (eex != null) {
                        var errors = eex.EntityValidationErrors;
                        foreach (var error in errors) {
                            Log($"ERROR FOR ENTITY: {error.Entry.Entity.GetType().Name}");
                            foreach (var errorMessage in error.ValidationErrors) {
                                Log($"    PROPERTY: " + errorMessage.PropertyName);
                                Log($"         MSG: " + errorMessage.ErrorMessage);
                                Log($"       VALUE: " + error.Entry.CurrentValues.GetValue<object>(errorMessage.PropertyName));
                            }
                        }
                    }
                }
                Log($"{Environment.NewLine}");
            }

            if (includeStackTrace) {
                Log($"{ex.StackTrace}");
                Log("");
            }
        }

        private void Log(string msg) {
            FileInfo logFile = new FileInfo(Path.Combine((string)AppDomain.CurrentDomain.GetData("DataDirectory"), "app.log"));
            if (!logFile.Exists)
                logFile.Create().Close();

            using (StreamWriter sw = logFile.AppendText())
                sw.WriteLine(msg);

            Debug.WriteLine(msg);
        }

        protected void AddIdentityResultErrors(IdentityResult result) {
            foreach (var error in result.Errors) {
                ModelState.AddModelError("", error);
            }
        }

        protected void AddModelStateErrors(bool useKey = true) {
            Dictionary<string, string[]> errDict = new Dictionary<string, string[]>();

            foreach (string key in ModelState.Keys) {
                string[] errors = ModelState[key].Errors.Select(e => e.ErrorMessage).ToArray();
                errDict.Add(key, errors);
            }

            foreach (string key in errDict.Keys) {
                foreach (string errMsg in errDict[key]) {
                    Debug.WriteLine($"ModelState Error [{key}]: {errMsg}");
                    ModelState.AddModelError(useKey ? key : "", errMsg);
                }
            }
        }
    }
}