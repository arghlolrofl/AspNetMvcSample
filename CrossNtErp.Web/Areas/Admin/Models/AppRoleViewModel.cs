using CrossNtErp.Web.Security;
using System.ComponentModel.DataAnnotations;

namespace CrossNtErp.Web.Areas.Admin.Models {
    public class AppRoleViewModel {
        public long Id { get; set; }

        [Required(
            ErrorMessageResourceType = typeof(Resources.Localization),
            ErrorMessageResourceName = "V_Name_Required")]
        [StringLength(96,
            ErrorMessageResourceType = typeof(Resources.Localization),
            ErrorMessageResourceName = "V_Name_Length",
            MinimumLength = 1)]
        [Display(
            ResourceType = typeof(Resources.Localization),
            Name = "P_Name")]
        public string Name { get; set; }

        public AppRoleViewModel() {

        }

        public AppRoleViewModel(AppRole role) {
            Id = role.Id;
            Name = role.Name;
        }
    }
}