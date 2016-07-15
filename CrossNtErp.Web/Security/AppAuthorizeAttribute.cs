using CrossNtErp.Web.Models;
using System;
using System.Diagnostics;
using System.Web.Mvc;

namespace CrossNtErp.Web.Security {
    public class AppAuthorizeAttribute : AuthorizeAttribute {
        public AccessRole AccessRoles { get; set; }


        public AppAuthorizeAttribute() : base() {

        }

        public AppAuthorizeAttribute(AccessRole roles) : this() {
            Roles = roles.ToString().Replace(" ", String.Empty);
            Debug.WriteLine("Set Roles property to: " + Roles);
        }
    }
}