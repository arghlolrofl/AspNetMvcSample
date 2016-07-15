using CrossNtErp.Shared.Entities.Finances;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CrossNtErp.Web.Areas.Finances.Models {
    public class CashJournalViewModel {
        public long Id { get; set; }

        [Display(Name = "Title", ResourceType = typeof(Resources.Localization))]
        [Required(
            ErrorMessageResourceType = typeof(Resources.Localization),
            ErrorMessageResourceName = "V_Required")]
        [StringLength(255,
            ErrorMessageResourceType = typeof(Resources.Localization),
            ErrorMessageResourceName = "V_Length_Max_255")]
        public string Title { get; set; }

        [Display(Name = "P_StartDate", ResourceType = typeof(Resources.Localization))]
        [Required(
            ErrorMessageResourceType = typeof(Resources.Localization),
            ErrorMessageResourceName = "V_Required")]
        public DateTime StartDate { get; set; }

        [Display(Name = "P_EndDate", ResourceType = typeof(Resources.Localization))]
        [Required(
            ErrorMessageResourceType = typeof(Resources.Localization),
            ErrorMessageResourceName = "V_Required")]
        public DateTime EndDate { get; set; }

        [Display(Name = "P_Description", ResourceType = typeof(Resources.Localization))]
        [StringLength(255,
            ErrorMessageResourceType = typeof(Resources.Localization),
            ErrorMessageResourceName = "V_Length_Max_255")]
        public string Description { get; set; }

        [Display(Name = "P_CurrentBalance", ResourceType = typeof(Resources.Localization))]
        public decimal CurrentBalance {
            get {
                return Entries.Sum(e => e.CashBalance);
            }
        }

        public long UserId { get; set; }

        public CurrencyViewModel DefaultCurrency { get; set; }


        public ICollection<CashJournalEntryViewModel> Entries { get; set; }


        public CashJournalViewModel() {
            Entries = new List<CashJournalEntryViewModel>();
        }

        public CashJournalViewModel(CashJournal journal) : this() {
            Id = journal.Id;
            Title = journal.Title;
            StartDate = journal.StartDate;
            EndDate = journal.EndDate;
            Description = journal.Description;
            UserId = journal.UserId;
            DefaultCurrency = journal.DefaultCurrency.ToViewModel();

            foreach (var entry in journal.Entries)
                Entries.Add(new CashJournalEntryViewModel(entry));
        }
    }
}