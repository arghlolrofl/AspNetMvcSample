using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace CrossNtErp.Web.Areas.Finances.Models {
    public class CurrencyViewModel {
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

        public string Symbol { get { return _cultureInfo.NumberFormat.CurrencySymbol; } }
        public string Name { get { return _regionInfo.CurrencyNativeName; } }
        public string ISO4217Code { get { return _regionInfo.ISOCurrencySymbol; } }
    }
}