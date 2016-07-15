using CrossNtErp.Shared.Entities.Finances;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;

namespace CrossNtErp.Web.Areas.Finances.Models {
    public class CashJournalEntryViewModel {
        public long Id { get; set; }

        [Display(Name = "P_Date", ResourceType = typeof(Resources.Localization))]
        [Required(
            ErrorMessageResourceType = typeof(Resources.Localization),
            ErrorMessageResourceName = "V_Required")]
        public DateTime Date { get; set; }

        [Display(Name = "P_DocumentName", ResourceType = typeof(Resources.Localization))]
        public string DocumentName { get; set; }

        [Display(Name = "P_ProcessDescription", ResourceType = typeof(Resources.Localization))]
        [Required(
            ErrorMessageResourceType = typeof(Resources.Localization),
            ErrorMessageResourceName = "V_Required")]
        public string ProcessDescription { get; set; }

        public long JournalId { get; set; }

        [Display(Name = "P_CashBalance", ResourceType = typeof(Resources.Localization))]
        public decimal CashBalance {
            get {
                return Positions.Sum(p => p.Delta);
            }
        }

        [Display(Name = "HasDocumentAttached", ResourceType = typeof(Resources.Localization))]
        public bool IsDocumentAttached {
            get {
                return !String.IsNullOrEmpty(DocumentName);
            }
        }

        [Display(Name = "PrePaidTax", ResourceType = typeof(Resources.Localization))]
        public decimal PrePaidTax { get; set; }


        public ICollection<CashJournalEntryPositionViewModel> Positions { get; set; }


        public CashJournalEntryViewModel() {
            Positions = new List<CashJournalEntryPositionViewModel>();
        }

        public CashJournalEntryViewModel(CashJournalEntry entry) : this() {
            Id = entry.Id;
            Date = entry.Date;
            DocumentName = entry.DocumentName;
            ProcessDescription = entry.ProcessDescription;
            JournalId = entry.JournalId;
            PrePaidTax = entry.PrePaidTax;            

            Debug.Assert(entry.Positions != null);

            for (int i = 0; i < entry.Positions.Count; i++)
                Positions.Add(new CashJournalEntryPositionViewModel(entry.Positions[i]) { Index = i });
        }
    }
}