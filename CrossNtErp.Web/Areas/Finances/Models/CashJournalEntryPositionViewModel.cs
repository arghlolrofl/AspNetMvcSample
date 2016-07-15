using CrossNtErp.Shared.Entities.Finances;
using System.ComponentModel.DataAnnotations;

namespace CrossNtErp.Web.Areas.Finances.Models {
    public class CashJournalEntryPositionViewModel {
        public long Index { get; set; }

        public long Id { get; set; }

        [Display(Name = "P_Description", ResourceType = typeof(Resources.Localization))]
        [Required(
            ErrorMessageResourceType = typeof(Resources.Localization),
            ErrorMessageResourceName = "V_MissingInput")]
        public string Description { get; set; }

        [Display(Name = "P_Delta", ResourceType = typeof(Resources.Localization))]
        [Required(
            ErrorMessageResourceType = typeof(Resources.Localization),
            ErrorMessageResourceName = "V_MissingInput")]
        [DataType(DataType.Currency)]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:c}", ConvertEmptyStringToNull = true)]
        public decimal Delta { get; set; }

        [Required(
            ErrorMessageResourceType = typeof(Resources.Localization),
            ErrorMessageResourceName = "V_MissingInput")]
        public long TaxRateId { get; set; }

        public TaxRateViewModel TaxRate { get; set; }

        [Required(
            ErrorMessageResourceType = typeof(Resources.Localization),
            ErrorMessageResourceName = "V_MissingInput")]
        public long EntryId { get; set; }

        [Display(Name = "PrePaidTax", ResourceType = typeof(Resources.Localization))]
        public decimal PrePaidTax { get; set; }


        public CashJournalEntryPositionViewModel() {

        }

        public CashJournalEntryPositionViewModel(CashJournalEntryPosition position) {
            Id = position.Id;
            Description = position.Description;
            Delta = position.Delta;
            TaxRateId = position.TaxRateId;
            EntryId = position.EntryId;
            PrePaidTax = position.PrepaidTax;
        }
    }
}