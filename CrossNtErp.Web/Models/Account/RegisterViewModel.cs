using System.ComponentModel.DataAnnotations;

namespace CrossNtErp.Web.Models.Account {
    public class RegisterViewModel {
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
            ErrorMessageResourceName = "V_Email_Required")]
        [Display(
            ResourceType = typeof(Resources.Localization),
            Name = "P_Email")]
        [EmailAddress]
        public string Email { get; set; }

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
            Name = "P_Password_Confirm")]
        [Compare("Password",
            ErrorMessageResourceType = typeof(Resources.Localization),
            ErrorMessageResourceName = "V_Password_Compare")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }

    public class RegisterAdminPasswordViewModel {
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
            Name = "P_Password_Confirm")]
        [Compare("Password",
            ErrorMessageResourceType = typeof(Resources.Localization),
            ErrorMessageResourceName = "V_Password_Compare")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}