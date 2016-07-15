using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace CrossNtErp.Web.Security {
    public class AppUserStore<TUser>
        : UserStore<TUser, AppRole, long, AppUserLogin, AppUserRole, AppUserClaim>,
        IUserStore<TUser>,
        IUserStore<TUser, long>,
        IDisposable
        where TUser : AppUser {
        public AppUserStore(DbContext context) : base(context) {

        }

        public Task<TUser> FindByIdAsync(string userId) {
            long id;
            if (!Int64.TryParse(userId, out id))
                throw new ArgumentException("Error converting string 'userId' to Int64");

            return base.FindByIdAsync(id);
        }
    }
}