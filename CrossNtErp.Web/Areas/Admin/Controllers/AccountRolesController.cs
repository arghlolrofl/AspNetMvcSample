using CrossNtErp.Web.Areas.Admin.Models;
using CrossNtErp.Web.Controllers.Base;
using CrossNtErp.Web.Models;
using CrossNtErp.Web.Resources;
using CrossNtErp.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CrossNtErp.Web.Areas.Admin.Controllers {
    [AppAuthorize(AccessRole.Admin)]
    public class AccountRolesController : LocalizationControllerBase {

        private AppRoleManager _roleManager;
        public AppRoleManager RoleManager {
            get {
                return _roleManager ?? (_roleManager = new AppRoleManager(new AppRoleStore<AppRole>(new AppIdentityContext())));
            }
        }


        /// <summary>
        /// GET: Admin/AccountRoles
        /// </summary>
        /// <returns></returns>
        public ActionResult Index() {
            List<AppRoleViewModel> roles = new List<AppRoleViewModel>();
            foreach (AppRole role in RoleManager.Roles.ToArray())
                roles.Add(new AppRoleViewModel(role));

            return View(roles);
        }

        #region Create

        /// <summary>
        /// GET: /Admin/AccountRoles/Create
        /// </summary>
        /// <returns></returns>
        public ActionResult Create() {
            return View(new AppRoleViewModel());
        }

        /// <summary>
        /// POST: /Admin/AccountRoles/Create
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Name")] AppRoleViewModel role) {
            if (!ModelState.IsValid) {
                AddModelStateErrors();
                return View(role);
            }

            // check if role exists already
            if (RoleManager.RoleExistsAsync(role.Name).Result) {
                ModelState.AddModelError("Name", Localization.RoleNameExistsAlready);
                return View(role);
            }

            // create the role
            AppRole newRole = new AppRole() { Name = role.Name };
            IdentityResult result = await RoleManager.CreateAsync(newRole);
            if (!result.Succeeded) {
                AddIdentityResultErrors(result);
                return View(role);
            }

            return RedirectToAction("Index");

        }

        #endregion

        #region Edit

        /// <summary>
        /// GET: /AccountRoles/Edit/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Edit(long? id) {
            if (!id.HasValue || id.Value <= 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            AppRole role = await RoleManager.FindByIdAsync(id.Value);
            if (role == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            return View(new AppRoleViewModel(role));
        }

        /// <summary>
        /// POST: /AccountRoles/Edit
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name")] AppRoleViewModel role) {
            if (!ModelState.IsValid) {
                AddModelStateErrors();
                return View(role);
            }

            AppRole existingRole = await RoleManager.FindByIdAsync(role.Id);
            if (existingRole == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            if (role.Name != existingRole.Name) {
                existingRole.Name = role.Name;

                await RoleManager.UpdateAsync(existingRole);
            }

            return RedirectToAction("Index");
        }

        #endregion

        #region Delete

        /// <summary>
        /// GET: /AccountRoles/Delete/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Delete(long? id) {
            if (!id.HasValue || id.Value <= 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            AppRole existingRole = await RoleManager.FindByIdAsync(id.Value);
            if (existingRole == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            return View(new AppRoleViewModel(existingRole));
        }

        /// <summary>
        /// POST: /AccountRoles/Delete
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Delete([Bind(Include = "Id")] AppRoleViewModel role) {
            AppUserManager userManager = HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            var roleToDelete = await RoleManager.FindByIdAsync(role.Id);
            if (roleToDelete == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            IdentityResult result = await RoleManager.DeleteAsync(roleToDelete);
            if (!result.Succeeded) {
                AddIdentityResultErrors(result);
                return View(role);
            }

            return RedirectToAction("Index");
        }
        #endregion
    }
}
