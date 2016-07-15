using CrossNtErp.Shared.Entities.Common;
using CrossNtErp.Web.Areas.Finances.Models;
using Microsoft.AspNet.Identity;
using System;

namespace CrossNtErp {
    public static class Extensions {

        /// <summary>
        /// Fetches the user id as string of the currently logged in user and casts
        /// it to <see cref="Int64"/>.
        /// </summary>
        /// <param name="principal"></param>
        /// <returns>AppUser's Id</returns>
        public static long GetId(this System.Security.Principal.IPrincipal principal) {
            string userId = principal.Identity.GetUserId();
            long id;
            if (!Int64.TryParse(userId, out id))
                throw new ArgumentException("Unable to cast 'string' to 'Int64': " + userId);

            return id;
        }


        public static CurrencyViewModel ToViewModel(this Currency currency) {
            CurrencyViewModel vm = new CurrencyViewModel();
            vm.CultureName = currency.CultureName;

            return vm;
        }
    }
}