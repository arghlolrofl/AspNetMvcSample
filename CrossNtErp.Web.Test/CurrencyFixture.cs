using CrossNtErp.Shared.Entities.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;

namespace CrossNtErp.Web.Test {
    [TestClass]
    public class CurrencyFixture {
        [TestMethod]
        public void CultureValidation_deDE() {
            string culture = "de-DE";
            string symbol = "€";
            string name = "Euro";
            string iso = "EUR";

            Currency currency = new Currency() { CultureName = culture };

            Assert.AreEqual(culture, currency.CultureName);
            Assert.AreEqual(symbol, currency.Symbol);
            Assert.AreEqual(name, currency.Name);
            Assert.AreEqual(iso, currency.ISO4217Code);
        }

        [TestMethod]
        public void CultureValidation_enUS() {
            string culture = "en-US";
            string symbol = "$";
            string name = "US Dollar";
            string iso = "USD";

            Currency currency = new Currency() { CultureName = culture };

            Assert.AreEqual(culture, currency.CultureName);
            Assert.AreEqual(symbol, currency.Symbol);
            Assert.AreEqual(name, currency.Name);
            Assert.AreEqual(iso, currency.ISO4217Code);
        }

        [TestMethod]
        public void CultureValidation_enGB() {
            string culture = "en-GB";
            string symbol = "£";
            string name = "Pound Sterling";
            string iso = "GBP";

            Currency currency = new Currency() { CultureName = culture };

            Assert.AreEqual(culture, currency.CultureName);
            Assert.AreEqual(symbol, currency.Symbol);
            Assert.AreEqual(name, currency.Name);
            Assert.AreEqual(iso, currency.ISO4217Code);
        }

        [TestMethod]
        [ExpectedException(typeof(CultureNotFoundException))]
        public void CurrencyCultureInfoIsNullForInvalidCultureNames() {
            string culture = "de-ZZ";

            Currency currency = new Currency() { CultureName = culture };
        }
    }
}
