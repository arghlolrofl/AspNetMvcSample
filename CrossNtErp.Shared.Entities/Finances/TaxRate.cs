using CrossNtErp.Shared.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace CrossNtErp.Shared.Entities.Finances {
    public class TaxRate : EntityBase {
        private string _category;
        private decimal _value;
        private string _description;

        [Required]
        public string Category {
            get { return _category; }
            set { _category = value; }
        }

        [Required]
        public decimal Value {
            get { return _value; }
            set { _value = value; }
        }

        public string Description {
            get { return _description; }
            set { _description = value; }
        }


        public override string ToString() {
            return $"{Value.ToString().PadLeft(5, '0')} % - {Category}";
        }
    }
}