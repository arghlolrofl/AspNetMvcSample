using CrossNtErp.Shared.Entities.Finances;
using System.ComponentModel.DataAnnotations;

namespace CrossNtErp.Web.Areas.Finances.Models {
    public class TaxRateViewModel {
        public long Id { get; set; }

        [Display(Name = "Category", ResourceType = typeof(Resources.Localization))]
        [Required(
            ErrorMessageResourceType = typeof(Resources.Localization),
            ErrorMessageResourceName = "V_Category_Required")]
        public string Category { get; set; }


        [Display(Name = "PercentageValue", ResourceType = typeof(Resources.Localization))]
        [Required(
            ErrorMessageResourceType = typeof(Resources.Localization),
            ErrorMessageResourceName = "V_Value_Required")]
        public decimal Value { get; set; }

        [Display(Name = "P_Description", ResourceType = typeof(Resources.Localization))]
        [StringLength(255,
            ErrorMessageResourceType = typeof(Resources.Localization),
            ErrorMessageResourceName = "V_Description_Length")]
        public string Description { get; set; }



        public TaxRateViewModel() {

        }

        public TaxRateViewModel(TaxRate taxRate) {
            Id = taxRate.Id;
            Category = taxRate.Category;
            Value = taxRate.Value;
            Description = taxRate.Description;
        }
    }
}