using CrossNtErp.Web.Security;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CrossNtErp.Web.Areas.Admin.Models {
    public class AppUserViewModel {
        public long Id { get; set; }

        [Required(
            ErrorMessageResourceType = typeof(Resources.Localization),
            ErrorMessageResourceName = "V_Username_Required")]
        [StringLength(32,
            ErrorMessageResourceType = typeof(Resources.Localization),
            ErrorMessageResourceName = "V_Username_Length",
            MinimumLength = 4)]
        [Display(
            ResourceType = typeof(Resources.Localization),
            Name = "P_Username")]
        public string UserName { get; set; }

        [Required(
            ErrorMessageResourceType = typeof(Resources.Localization),
            ErrorMessageResourceName = "V_Email_Required")]
        [Display(
            ResourceType = typeof(Resources.Localization),
            Name = "P_Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(
            ResourceType = typeof(Resources.Localization),
            Name = "P_PhoneNumber")]
        public string PhoneNumber { get; set; }

        public AppUserViewModel() {

        }

        public AppUserViewModel(AppUser user) {
            Id = user.Id;
            UserName = user.UserName;
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
        }


        public List<AppRole> AssignedRoles { get; set; }
        public List<AppRole> AvailableRoles { get; set; }
        public List<AppUserRole> Roles { get; set; }
    }
}