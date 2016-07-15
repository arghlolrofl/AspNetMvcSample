using CrossNtErp.Shared.Entities.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace CrossNtErp.Shared.Entities.Common {
    public class Currency : EntityBase {
        private CultureInfo _cultureInfo;
        private RegionInfo _regionInfo;

        [Required]
        public string CultureName {
            get { return _cultureInfo == null ? null : _cultureInfo.Name; }
            set {
                CultureInfo ci = CultureInfo.GetCultureInfo(value);
                if (ci != null) {
                    _cultureInfo = ci;
                    _regionInfo = new RegionInfo(value);
                }
            }
        }

        [NotMapped]
        public string Symbol { get { return _cultureInfo.NumberFormat.CurrencySymbol; } }
        [NotMapped]
        public string Name { get { return _regionInfo.CurrencyNativeName; } }
        [NotMapped]
        public string ISO4217Code { get { return _regionInfo.ISOCurrencySymbol; } }
    }
}
