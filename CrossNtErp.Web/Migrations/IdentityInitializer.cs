using CrossNtErp.Web.Security;
using System.Data.Entity;

namespace CrossNtErp.Web.Migrations {
    /// <summary>
    /// Database Initializer defined in Web.config
    /// </summary>
    internal class IdentityInitializer : MigrateDatabaseToLatestVersion<AppIdentityContext, Configuration> {

    }
}