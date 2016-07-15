using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace CrossNtErp.Web.Security {
    public class AppRoleManager : RoleManager<AppRole, long> {
        public AppRoleManager(IRoleStore<AppRole, long> roleStore)
            : base(roleStore) {
        }

        public static AppRoleManager Create(IdentityFactoryOptions<AppRoleManager> options, IOwinContext context) {
            return new AppRoleManager(new AppRoleStore<AppRole>(context.Get<AppIdentityContext>()));
        }
    }
}