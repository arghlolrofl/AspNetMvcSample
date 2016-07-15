using CrossNtErp.Web.Areas.Admin.Models;
using CrossNtErp.Web.Controllers.Base;
using CrossNtErp.Web.Models;
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
    public class UsersController : LocalizationControllerBase {
        private AppIdentityContext db = new AppIdentityContext();

        private AppRoleManager _roleManager;
        public AppRoleManager RoleManager {
            get {
                return _roleManager ?? (_roleManager = new AppRoleManager(new AppRoleStore<AppRole>(db)));
            }
        }

        private AppUserManager _userManager;
        public AppUserManager UserManager {
            get {
                return _userManager ?? (_userManager = HttpContext.GetOwinContext().GetUserManager<AppUserManager>());
            }
        }


        // GET: Users
        public ActionResult Index() {
            List<AppUserViewModel> users = new List<AppUserViewModel>();
            foreach (AppUser user in UserManager.Users.ToArray())
                users.Add(new AppUserViewModel(user));

            return View(users);
        }

        public async Task<ActionResult> Edit(long? id) {
            if (!id.HasValue || id.Value <= 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            AppUser userToEdit = await UserManager.FindByIdAsync(id.Value);
            if (userToEdit == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            IEnumerable<long> roleIds = from ur in userToEdit.Roles
                                        select ur.RoleId;

            List<AppRole> userRoles = (from r in RoleManager.Roles
                                       where roleIds.Contains(r.Id)
                                       select r).ToList();

            var vm = new AppUserViewModel(userToEdit);
            vm.AvailableRoles = RoleManager.Roles.ToArray().Except(userRoles).ToList();
            vm.AssignedRoles = userRoles;

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,UserName,Email,PhoneNumber,Roles")] AppUserViewModel user) {
            AppUser userFromDb = await UserManager.FindByIdAsync(user.Id);

            // update scalar properties ...
            userFromDb.UserName = user.UserName;
            userFromDb.Email = user.Email;
            userFromDb.PhoneNumber = user.PhoneNumber;

            // ... and persist the user
            IdentityResult result = await UserManager.UpdateAsync(userFromDb);
            if (!result.Succeeded) {
                AddIdentityResultErrors(result);
                return View(user);
            }

            // gather names of assigned roles ...
            IList<long> roleIds = userFromDb.Roles.Select(r => r.RoleId).ToList();
            string[] roleNames = RoleManager.Roles.Where(r => roleIds.Contains(r.Id)).Select(r => r.Name).ToArray();

            // ... and remove user from assigned roles
            result = UserManager.RemoveFromRoles(userFromDb.Id, roleNames);
            if (!result.Succeeded) {
                AddIdentityResultErrors(result);
                return View(user);
            }

            // gather role names to be assigned ...
            roleNames = new string[user.Roles.Count];
            for (int i = 0; i < user.Roles.Count; i++)
                roleNames[i] = RoleManager.FindByIdAsync(user.Roles[i].RoleId).Result.Name;

            // ... and assign them to the user
            result = await UserManager.AddToRolesAsync(userFromDb.Id, roleNames);
            if (!result.Succeeded) {
                AddIdentityResultErrors(result);
                return View(user);
            }

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Delete(long? id) {
            if (!id.HasValue || id.Value <= 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            AppUser user = await UserManager.FindByIdAsync(id.Value);
            if (user == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            return View(new AppUserViewModel(user));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<ActionResult> ConfirmDelete([Bind(Include = "Id")] AppUserViewModel user) {
            AppUser userFromDb = await UserManager.FindByIdAsync(user.Id);
            IdentityResult result = await UserManager.DeleteAsync(userFromDb);
            if (!result.Succeeded) {
                AddIdentityResultErrors(result);
                return View(user);
            }

            return RedirectToAction("Index");
        }
    }
}