using CrossNtErp.Shared.Entities.Base;
using CrossNtErp.Shared.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace CrossNtErp.Shared.Entities.Finances {
    public class CashJournal : EntityBase {
        private ICollection<CashJournalEntry> _entries = new List<CashJournalEntry>();

        /// <summary>
        /// Owner's user id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// A sequential number
        /// </summary>
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// Start date of the journal
        /// </summary>
        [Required]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// End date of the journal
        /// </summary>
        [Required]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Description of the journal (Kassenbuchbeschreibung)
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Sum of balances of all entries.
        /// </summary>
        public decimal CurrentBalance {
            get { return Entries.Sum(e => e.CashBalance); }
        }

        public bool IsVisibleToEverybody { get; set; }

        public long DefaultCurrencyId { get; set; }
        [ForeignKey(nameof(DefaultCurrencyId))]
        public Currency DefaultCurrency { get; set; }

        /// <summary>
        /// List of journal entries
        /// </summary>
        public virtual ICollection<CashJournalEntry> Entries {
            get { return _entries; }
            set { _entries = value; }
        }
    }
}