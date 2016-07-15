using CrossNtErp.Web.Models;
using CrossNtErp.Web.Security;
using Microsoft.AspNet.Identity;
using System.Data.Entity.Migrations;

namespace CrossNtErp.Web.Migrations {
    /// <summary>
    /// Migrations Configuration used by <see cref="IdentityInitializer"/>
    /// Debug:
    ///     Seeds all available roles and 3 users with different roles for testing
    ///     
    /// Release:
    ///     Seeds all available roles and NO USERS! The first registered user is then 
    ///     being assigned the admin role.
    /// 
    /// </summary>
    internal sealed class Configuration : DbMigrationsConfiguration<AppIdentityContext> {
        const string AdminUserName = "Admin";
        const string AdminUserPassword = "Admin@123456";
        const string AdminUserMail = "admin@example.com";

        const string DefaultUserName = "Default";
        const string DefaultUserPassword = "Test123!";
        const string DefaultUserMail = "default@example.com";

        const string OutlawUserName = "Outlaw";
        const string OutlawUserPassword = "Test123!";
        const string OutlawUserMail = "outlaw@example.com";


        public Configuration() {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(AppIdentityContext context) {
            AppRoleManager roleManager = new AppRoleManager(new AppRoleStore<AppRole>(context));
            if (roleManager.RoleExists(nameof(AccessRole.Admin)))
                return;

            try {
                SeedRoles(roleManager);
            } catch (RoleSeedException rEx) {
                throw new DataSeedException("Error while seeding Identity Roles", rEx);
            }

#if DEBUG
            AppUserManager userManager = new AppUserManager(new AppUserStore<AppUser>(context));

            try {
                SeedUser(userManager, AdminUserName, AdminUserMail, AdminUserPassword,
                    nameof(AccessRole.Admin),
                    nameof(AccessRole.User),
                    nameof(AccessRole.Finances),
                    nameof(AccessRole.CashJournal),
                    nameof(AccessRole.TaxRate));

                SeedUser(userManager, DefaultUserName, DefaultUserMail, DefaultUserPassword,
                    nameof(AccessRole.User),
                    nameof(AccessRole.Finances),
                    nameof(AccessRole.CashJournal),
                    nameof(AccessRole.TaxRate));

                SeedUser(userManager, OutlawUserName, OutlawUserMail, OutlawUserPassword,
                    nameof(AccessRole.User));

            } catch (UserSeedException uEx) {
                throw new DataSeedException("Error while seeding Identity Users", uEx);
            }
#endif
        }

        private async void SeedRoles(AppRoleManager roleManager) {
            string errorMessage = "Failed to create role!";

            // Admin roles
            IdentityResult result = await roleManager.CreateAsync(new AppRole() { Name = nameof(AccessRole.Admin) });
            if (!result.Succeeded)
                throw new RoleSeedException(nameof(AccessRole.Admin), errorMessage);

            // Default role
            result = await roleManager.CreateAsync(new AppRole() { Name = nameof(AccessRole.User) });
            if (!result.Succeeded)
                throw new RoleSeedException(nameof(AccessRole.User), errorMessage);

            // Finances roles
            result = await roleManager.CreateAsync(new AppRole() { Name = nameof(AccessRole.Finances) });
            if (!result.Succeeded)
                throw new RoleSeedException(nameof(AccessRole.Finances), errorMessage);

            result = await roleManager.CreateAsync(new AppRole() { Name = nameof(AccessRole.CashJournal) });
            if (!result.Succeeded)
                throw new RoleSeedException(nameof(AccessRole.CashJournal), errorMessage);

            result = await roleManager.CreateAsync(new AppRole() { Name = nameof(AccessRole.TaxRate) });
            if (!result.Succeeded)
                throw new RoleSeedException(nameof(AccessRole.TaxRate), errorMessage);
        }

        private void SeedUser(AppUserManager userManager, string name, string mail, string password, params string[] roles) {
            PasswordHasher hasher = new PasswordHasher();

            AppUser user = new AppUser() {
                UserName = name,
                Email = mail,
                PasswordHash = hasher.HashPassword(password)
            };

            userManager.CreateAsync(user);
            userManager.AddToRolesAsync(user.Id, roles);
        }
    }
}