using System.ComponentModel.DataAnnotations;

namespace CrossNtErp.Web.Models.Account {
    public class LoginViewModel {
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
        public string Username { get; set; }


        [Required(
            ErrorMessageResourceType = typeof(Resources.Localization),
            ErrorMessageResourceName = "V_Password_Required")]
        [StringLength(32,
            ErrorMessageResourceType = typeof(Resources.Localization),
            ErrorMessageResourceName = "V_Password_Length",
            MinimumLength = 4)]
        [Display(
            ResourceType = typeof(Resources.Localization),
            Name = "P_Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Display(
            ResourceType = typeof(Resources.Localization),
            Name = "P_RememberMe")]
        public bool RememberMe { get; set; }
    }
}