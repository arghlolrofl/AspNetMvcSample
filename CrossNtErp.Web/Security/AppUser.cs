using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CrossNtErp.Web.Security {
    public class AppUser : IdentityUser<long, AppUserLogin, AppUserRole, AppUserClaim>, IUser, IUser<long> {
        string IUser<string>.Id {
            get {
                return Id.ToString();
            }
        }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<AppUser, long> manager) {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}