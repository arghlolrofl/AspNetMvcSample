using CrossNtErp.Shared.Entities.Base;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace CrossNtErp.Shared.Entities.Finances {
    public class CashJournalEntry : EntityBase {
        private DateTime _date;
        private string _documentName;
        private string _processDescription;
        private CashJournal _journal;
        private ObservableCollection<CashJournalEntryPosition> _positions = new ObservableCollection<CashJournalEntryPosition>();


        /// <summary>
        /// Name of the document file
        /// </summary>
        public string DocumentName {
            get { return _documentName; }
            set { _documentName = value; }
        }

        /// <summary>
        /// DocumentDate (Belegdatum)
        /// </summary>
        [Required]
        public DateTime Date {
            get { return _date; }
            set { _date = value; }
        }

        /// <summary>
        /// Business process (Geschäftsvorgangsbeschreibung)
        /// </summary>
        [Required]
        public string ProcessDescription {
            get { return _processDescription; }
            set { _processDescription = value; }
        }

        /// <summary>
        /// Current cash balance (Bestand)
        /// </summary>
        [Required]
        public decimal CashBalance {
            get { return Positions.Sum(position => position.Delta); }
            //set { _cashBalance = value; }
        }

        /// <summary>
        /// Journal Foreign Key
        /// </summary>        
        public long JournalId { get; set; }

        /// <summary>
        /// Navigation property to <see cref="Entities.CashJournal.CashJournal"/>
        /// </summary>
        [Required]
        [ForeignKey(nameof(JournalId))]
        public virtual CashJournal Journal {
            get { return _journal; }
            set { _journal = value; }
        }

        /// <summary>
        /// List of positions for this entry.
        /// </summary>
        public virtual ObservableCollection<CashJournalEntryPosition> Positions {
            get { return _positions; }
            set { _positions = value; }
        }

        /// <summary>
        /// PrePaid Tax (Vorsteuer)
        /// </summary>
        public decimal PrePaidTax {
            get {
                if (Positions == null || !Positions.Any())
                    return 0.00m;

                return Positions.Sum(p => p.PrepaidTax);
            }
        }

    }
}