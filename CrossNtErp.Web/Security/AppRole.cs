using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;

namespace CrossNtErp.Web.Security {
    public class AppRole : IdentityRole<long, AppUserRole>, IRole, IRole<long> {
        string IRole<string>.Id {
            get {
                return Id.ToString();
            }
        }

        public override bool Equals(object obj) {
            if (obj == null)
                return false;

            AppRole roleToCompare = obj as AppRole;
            if (roleToCompare == null)
                return false;

            return roleToCompare.Id == Id;
        }

        public override int GetHashCode() {
            return String.Format("{0}_{1}", Id, Name).GetHashCode();
        }
    }
}