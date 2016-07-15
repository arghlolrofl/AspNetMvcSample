using Microsoft.AspNet.Identity.EntityFramework;

namespace CrossNtErp.Web.Security {
    public class AppIdentityContext : IdentityDbContext<AppUser, AppRole, long, AppUserLogin, AppUserRole, AppUserClaim> {
        public static AppIdentityContext Create() => new AppIdentityContext();

        public AppIdentityContext() : base("IdentityConnection") { }
    }
}