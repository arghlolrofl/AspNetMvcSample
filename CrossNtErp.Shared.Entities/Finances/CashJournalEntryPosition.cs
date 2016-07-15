using CrossNtErp.Shared.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace CrossNtErp.Shared.Entities.Finances {
    public class CashJournalEntryPosition : EntityBase {
        private decimal _delta;
        private string _description;
        private long _taxRateId;
        private long _entryId;
        private decimal _prepaidTax;
        private TaxRate _taxRate;
        private CashJournalEntry _entry;

        [Required]
        public decimal Delta {
            get { return Math.Round(_delta, 2); }
            set { _delta = value; CalculatePrepaidTax(); }
        }

        public string Description {
            get { return _description; }
            set { _description = value; }
        }

        public decimal PrepaidTax {
            get { return _prepaidTax; }
            set { _prepaidTax = value; }
        }

        [Required]
        public long TaxRateId {
            get { return _taxRateId; }
            set { _taxRateId = value; }
        }

        [Required]
        public virtual TaxRate TaxRate {
            get { return _taxRate; }
            set { _taxRate = value; CalculatePrepaidTax(); }
        }

        public long EntryId {
            get { return _entryId; }
            set { _entryId = value; }
        }


        [Required]
        public virtual CashJournalEntry Entry {
            get { return _entry; }
            set { _entry = value; }
        }


        private void CalculatePrepaidTax() {
            if (_delta == 0 || TaxRate == null) {
                PrepaidTax = 0.00m;
            } else {
                decimal divisor = 100 + TaxRate.Value;
                decimal prepaidTax = _delta - (_delta / divisor * 100);
                PrepaidTax = Math.Round(prepaidTax, 2);
            }
        }
    }
}
