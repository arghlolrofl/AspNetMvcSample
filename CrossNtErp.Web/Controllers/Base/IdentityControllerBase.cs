using CrossNtErp.Web.Auth;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CrossNtErp.Web.Base {
    public abstract class IdentityControllerBase : LocalizationControllerBase {
        private ApplicationRoleManager _roleManager;
        private UserManager<ApplicationUser> _userManager;

        public string CurrentUserId { get; set; }
        public ApplicationRoleManager RoleManager {
            get {
                if (_roleManager == null)
                    _roleManager = System.Web.HttpContext.Current
                                                         .GetOwinContext()
                                                         .Get<ApplicationRoleManager>();

                return _roleManager;
            }
        }

        public UserManager<ApplicationUser> UserManager {
            get {
                if (_userManager == null) {
                    var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
                    _userManager = new UserManager<ApplicationUser>(store);
                }

                return _userManager;
            }
        }


        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            CurrentUserId = GetAthenticatedUserId();
        }

        internal void SaveRoleInformation(ApplicationUser userFromDb, ApplicationUser userFromForm) {
            var assignedRoles = new List<string>();
            foreach (var role in userFromDb.Roles)
                assignedRoles.Add(RoleManager.FindById(role.RoleId).Name);

            UserManager.RemoveFromRoles(userFromDb.Id, assignedRoles.ToArray());

            foreach (var role in userFromForm.Roles)
                AddUserToRole(userFromDb.Id, role);
        }

        internal void SaveUserInformation(ApplicationUser userFromDb, ApplicationUser user) {
            if (userFromDb.Email != user.Email ||
                userFromDb.PhoneNumber != user.PhoneNumber ||
                userFromDb.UserName != user.UserName) {

                userFromDb.Email = user.Email;
                userFromDb.PhoneNumber = user.PhoneNumber;
                userFromDb.UserName = user.UserName;

                UserManager.Update(userFromDb);
            }
        }

        private string GetAthenticatedUserId() {
            if (User == null || !User.Identity.IsAuthenticated)
                return null;
            ApplicationUser user = UserManager.FindByNameAsync(User.Identity.Name).Result;

            return user.Id;
        }

        internal bool AddUserToRole(string userId, IdentityUserRole role) {
            IdentityRole availableRole = RoleManager.FindById(role.RoleId);
            IdentityResult result = UserManager.AddToRole(userId, availableRole.Name);

            return result.Succeeded;
        }

        protected virtual IList<ApplicationUser> GetUsers() {
            return UserManager.Users.ToList();
        }

        protected virtual ApplicationUser GetUserById(string userId) {
            return UserManager.Users.SingleOrDefault(user => user.Id == userId);
        }

        internal async Task<IdentityResult> CreateNewRole(ApplicationRole role) {
            return await RoleManager.CreateAsync(role);
        }

        protected virtual IList<ApplicationRole> GetRoles() {
            return RoleManager.Roles.ToList();
        }

        protected async virtual Task<ApplicationRole> GetRoleById(string roleId) {
            return await RoleManager.FindByIdAsync(roleId);
        }

        protected void SaveChangedRoleName(ApplicationRole existingRole, string name) {
            existingRole.Name = name;
            RoleManager.Update(existingRole);
        }

        internal async Task<IdentityResult> DeleteRoleById(string roleId) {
            var role = RoleManager.FindById(roleId);
            return await RoleManager.DeleteAsync(role);
        }
    }
}