using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace CrossNtErp.Web.Security {
    public class AppRoleStore<TRole>
        : RoleStore<TRole, long, AppUserRole>,
        IQueryableRoleStore<TRole>,
        IQueryableRoleStore<TRole, long>,
        IRoleStore<TRole, long>,
        IDisposable
        where TRole : AppRole, new() {
        public AppRoleStore(DbContext context) : base(context) {

        }

        public Task<TRole> FindByIdAsync(string roleId) {
            long id;
            if (!Int64.TryParse(roleId, out id))
                throw new ArgumentException("Error converting string 'roleId' to Int64");

            return base.FindByIdAsync(id);
        }
    }
}