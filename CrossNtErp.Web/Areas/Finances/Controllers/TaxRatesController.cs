using CrossNtErp.DataAccess;
using CrossNtErp.Shared.Entities.Finances;
using CrossNtErp.Web.Areas.Finances.Models;
using CrossNtErp.Web.Controllers.Base;
using CrossNtErp.Web.Models;
using CrossNtErp.Web.Resources;
using CrossNtErp.Web.Security;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace CrossNtErp.Web.Areas.Finances.Controllers {
    [AppAuthorize(AccessRole.Admin | AccessRole.TaxRate)]
    public class TaxRatesController : LocalizationControllerBase {
        private DataContext db = new DataContext();

        /// <summary>
        /// GET: Finances/TaxRates
        /// </summary>
        /// <returns></returns>
        public ActionResult Index() {
            List<TaxRateViewModel> entities = new List<TaxRateViewModel>();
            foreach (TaxRate rate in db.TaxRates.ToArray())
                entities.Add(new TaxRateViewModel(rate));

            return View(entities);
        }

        /// <summary>
        /// GET: Finances/TaxRates/Details/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(long? id) {
            if (!id.HasValue || id.Value <= 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            TaxRate taxRate = db.TaxRates.Find(id);
            if (taxRate == null)
                return HttpNotFound();

            return View(new TaxRateViewModel(taxRate));
        }

        /// <summary>
        /// GET: Finances/TaxRates/Create
        /// </summary>
        /// <returns></returns>
        public ActionResult Create() => View();

        // POST: Finances/TaxRates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Category,Value,Description")] TaxRateViewModel taxRate) {
            if (ModelState.IsValid) {

                TaxRate newTaxRate = new TaxRate {
                    Category = taxRate.Category,
                    Value = taxRate.Value,
                    Description = taxRate.Description
                };

                db.TaxRates.Add(newTaxRate);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            LocalizeModelStateError(nameof(taxRate.Value), Localization.InvalidPercentageValue);

            return View(taxRate);
        }

        /// <summary>
        /// GET: Finances/TaxRates/Edit/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(long? id) {
            if (!id.HasValue || id.Value <= 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            TaxRate taxRate = db.TaxRates.Find(id);
            if (taxRate == null)
                return HttpNotFound();

            return View(new TaxRateViewModel(taxRate));
        }

        // POST: Finances/TaxRates/Edit/{id}
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Category,Value,Description")] TaxRateViewModel taxRate) {
            if (ModelState.IsValid) {
                TaxRate rateToUpdate = db.TaxRates.Find(taxRate.Id);
                if (rateToUpdate == null)
                    return HttpNotFound();

                rateToUpdate.Category = taxRate.Category;
                rateToUpdate.Description = taxRate.Description;
                rateToUpdate.Value = taxRate.Value;

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            LocalizeModelStateError(nameof(taxRate.Value), Localization.InvalidPercentageValue);

            return View(taxRate);
        }

        /// <summary>
        /// GET: Finances/TaxRates/Delete/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(long? id) {
            if (!id.HasValue || id.Value <= 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            TaxRate taxRate = db.TaxRates.Find(id);
            if (taxRate == null)
                return HttpNotFound();

            return View(new TaxRateViewModel(taxRate));
        }

        /// <summary>
        /// POST: Finances/TaxRates/Delete/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id) {
            TaxRate taxRate = db.TaxRates.Find(id);
            db.TaxRates.Remove(taxRate);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing) {
            if (disposing)
                db.Dispose();

            base.Dispose(disposing);
        }

        /// <summary>
        /// Deletes default validation messages, when the model binder is unable to cast a
        /// input string into e.g. a decimal property and sets a localized error message.
        /// </summary>
        /// <param name="modelStateKey">Name of the property in ModelState to look for unlocalized error message</param>
        /// <param name="errorMessage">Localized error message</param>
        private void LocalizeModelStateError(string modelStateKey, string errorMessage) {
            if (ModelState.ContainsKey(modelStateKey)) {
                var errors = ModelState[modelStateKey].Errors;
                errors.Clear();
                errors.Add(errorMessage);
            }
        }
    }
}
