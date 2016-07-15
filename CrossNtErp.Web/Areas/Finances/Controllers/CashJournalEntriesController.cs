using CrossNtErp.DataAccess;
using CrossNtErp.Shared.Entities.Finances;
using CrossNtErp.Web.Areas.Finances.Models;
using CrossNtErp.Web.Controllers.Base;
using CrossNtErp.Web.Models;
using CrossNtErp.Web.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Resources;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace CrossNtErp.Web.Areas.Finances.Controllers {
    [AppAuthorize(AccessRole.CashJournal)]
    public class CashJournalEntriesController : LocalizationControllerBase {
        private DataContext db = new DataContext();
        private SelectList availableTaxRates;

        public SelectList AvailableTaxRates {
            get {
                if (availableTaxRates == null) {
                    var taxRates = from tr in db.TaxRates
                                   select new TaxRateViewModel { Id = tr.Id, Category = tr.Category };

                    availableTaxRates = new SelectList(taxRates.ToList(), "Id", "Category");
                }

                return availableTaxRates;
            }
        }

        /// <summary>
        /// GET: Finances/CashJournalEntries/Create/{CashJournalId}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Create(long? id, int? positionCount) {
            if (!id.HasValue || id.Value <= 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            CashJournalEntryViewModel vm = new CashJournalEntryViewModel {
                JournalId = id.Value,
                Date = DateTime.Now,
                Positions = new List<CashJournalEntryPositionViewModel>() { new CashJournalEntryPositionViewModel { Index = 0 } }
            };

            ModelState.Clear();

            ViewBag.TaxRates = AvailableTaxRates;

            return View(vm);
        }

        /// <summary>
        /// POST: Finances/CashJournalEntries/Create
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DocumentName,Date,ProcessDescription,JournalId,Positions,IsDocumentAttached")] CashJournalEntryViewModel entry) {
            if (ShouldAddPosition(entry, Request["AddPosition"]) || ShouldRemovePosition(entry, Request["RemovePosition"])) {
                ViewBag.TaxRates = AvailableTaxRates;
                return View(entry);
            }

            if (!entry.IsDocumentAttached)
                entry.DocumentName = null;

            ValidateDeltaOfPositions(entry);

            if (!ModelState.IsValid) {
                ViewBag.TaxRates = AvailableTaxRates;
                return View(entry);
            }

            CashJournalEntry newEntry = HydrateCashJournalEntry(entry);

            db.CashJournalEntries.Add(newEntry);
            db.SaveChanges();
            return RedirectToAction("Edit", "CashJournals", new { id = entry.JournalId });
        }

        /// <summary>
        /// GET: Finances/CashJournalEntries/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(long? id) {
            if (!id.HasValue || id.Value <= 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            CashJournalEntry cashJournalEntry = db.CashJournalEntries
                                                  .Include(cje => cje.Positions)
                                                  .SingleOrDefault(cje => cje.Id == id);
            if (cashJournalEntry == null)
                return HttpNotFound();

            ModelState.Clear();

            ViewBag.TaxRates = AvailableTaxRates;

            return View(new CashJournalEntryViewModel(cashJournalEntry));
        }

        /// <summary>
        /// POST: Finances/CashJournalEntries/Edit/5
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DocumentName,Date,ProcessDescription,JournalId,Positions,IsDocumentAttached")] CashJournalEntryViewModel entry) {
            if (ShouldAddPosition(entry, Request["AddPosition"]) || ShouldRemovePosition(entry, Request["RemovePosition"])) {
                ViewBag.TaxRates = AvailableTaxRates;
                return View(entry);
            }

            if (!entry.IsDocumentAttached && !String.IsNullOrEmpty(entry.DocumentName))
                entry.DocumentName = null;

            ValidateDeltaOfPositions(entry);

            if (!ModelState.IsValid) {
                // Workaround: When the model state is invalid, we need to prevent Razor from
                // using this specific cached value. Otherwise not only the entry id will receive
                // the cached value (which would be correct), but the position's id too.
                if (ModelState.ContainsKey("Id"))
                    ModelState.Remove("Id");

                ViewBag.TaxRates = AvailableTaxRates;
                return View(entry);
            }

            CashJournalEntry entryToUpdate = HydrateCashJournalEntry(entry);
            db.Entry(entryToUpdate).State = EntityState.Modified;

            foreach (var position in entryToUpdate.Positions) {
                if (position.Exists) {
                    db.CashJournalEntryPositions.Attach(position);
                    db.Entry(position).State = EntityState.Modified;
                } else {
                    db.Entry(position).State = EntityState.Added;
                }
            }

            db.SaveChanges();
            return RedirectToAction("Edit", "CashJournals", new { area = "Finances", id = entryToUpdate.JournalId });
        }

        /// <summary>
        /// GET: Finances/CashJournalEntries/Delete/{id}
        /// </summary>
        /// <param name="id">Cash Journal Entry Id</param>
        /// <returns></returns>
        public ActionResult Delete(long? id) {
            if (!id.HasValue || id.Value <= 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            CashJournalEntry cashJournalEntry = db.CashJournalEntries.Find(id);
            if (cashJournalEntry == null)
                return HttpNotFound();

            return View(cashJournalEntry);
        }

        /// <summary>
        /// POST: Finances/CashJournalEntries/Delete/{id}
        /// </summary>
        /// <param name="id">Cash Journal Entry Id</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id) {
            CashJournalEntry cashJournalEntry = db.CashJournalEntries.Find(id);
            long journalId = cashJournalEntry.JournalId;
            db.CashJournalEntries.Remove(cashJournalEntry);
            db.SaveChanges();

            return RedirectToAction("Edit", "CashJournals", new { id = journalId });
        }



        protected override void Dispose(bool disposing) {
            if (disposing) {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        private CashJournalEntry HydrateCashJournalEntry(CashJournalEntryViewModel entry) {
            CashJournalEntry entryEntity;

            if (entry.Id > 0) {
                entryEntity = db.CashJournalEntries
                                .Include(cje => cje.Positions)
                                .SingleOrDefault(cje => cje.Id == entry.Id);
            } else
                entryEntity = new CashJournalEntry();

            // load journal by id
            entryEntity.Date = entry.Date;
            entryEntity.DocumentName = entry.DocumentName;
            entryEntity.ProcessDescription = entry.ProcessDescription;
            entryEntity.Journal = db.CashJournals.Find(entry.JournalId);
            entryEntity.JournalId = entry.JournalId;

            entryEntity.Positions.Clear();

            for (int i = 0; i < entry.Positions.Count; i++)
                entryEntity = HydrateCashJournalEntryPosition(entryEntity, entry.Positions.ElementAt(i), i);

            return entryEntity;
        }

        private CashJournalEntry HydrateCashJournalEntryPosition(CashJournalEntry entry, CashJournalEntryPositionViewModel position, int i) {
            CashJournalEntryPosition entryPosition;

            if (position.Id <= 0)
                entryPosition = new CashJournalEntryPosition();
            else
                entryPosition = db.CashJournalEntryPositions.Find(position.Id);

            entryPosition.Delta = position.Delta;
            entryPosition.Description = position.Description;
            entryPosition.Entry = entry;
            entryPosition.EntryId = entry.Id;
            entryPosition.TaxRateId = position.TaxRateId;
            entryPosition.TaxRate = db.TaxRates.Single(tr => tr.Id == position.TaxRateId);

            entry.Positions.Add(entryPosition);

            return entry;
        }

        private bool ShouldAddPosition(CashJournalEntryViewModel entry, string posIdx) {
            bool posIndex;
            if (Boolean.TryParse(posIdx, out posIndex) && posIndex) {
                entry.Positions.Add(new CashJournalEntryPositionViewModel() { Index = entry.Positions.Count });

                ModelState.Clear();

                return true;
            }

            return false;
        }

        private bool ShouldRemovePosition(CashJournalEntryViewModel entry, string posIdx) {
            int positionIndex;
            if (Int32.TryParse(posIdx, out positionIndex) && (positionIndex >= 0 && positionIndex < entry.Positions.Count)) {
                if (entry.Positions.Count > 1) {
                    var pos = entry.Positions.ElementAt(positionIndex);
                    var id = pos.Id;
                    entry.Positions.Remove(pos);

                    if (id > 0) {
                        var positionInDb = db.CashJournalEntryPositions.Find(id);

                        Debug.Assert(positionInDb != null);

                        db.CashJournalEntryPositions.Remove(positionInDb);
                        db.SaveChanges();
                    }
                }

                ModelState.Clear();

                return true;
            }

            return false;
        }

        /// <summary>
        /// Validates 'Delta' property of every <see cref="CashJournalEntryPositionViewModel"/>. 
        /// The regular expression pattern for the correspondent culture is stored as resource string 
        /// 'Resources.Localization.CurrencyPattern'.
        /// 
        /// This is the easiest and most reliable solution I have found. Other solutions that I have tried:
        /// 
        /// 1. RegularExpression Attribute on ViewModel
        ///     -> No luck, when trying to pass a localized pattern into attribute ctor
        ///     
        /// 2. Custom RegularExpressionAttribute
        ///     -> Localized pattern has been used, but validation message was always the same and did not get localized.
        ///     
        /// 3. Custom ValidationAttribute
        ///     -> Mostly the same. Unlocalized default validation message. Would have needed to mess with ModelState directly as a workaround.
        ///     
        /// 
        /// Main problem seems to be, that one entry can have any number of positions. Therefore positions are POSTed as a collection:
        /// Entry ->
        ///     -> Position0                    Positions[0].Delta
        ///     -> Position1            ==>     Positions[1].Delta
        ///     -> Position2                    Positions[2].Delta
        ///     
        /// But when validating these values with a custom DataValidation attribute, the propertyName is simply
        /// 'Delta' and not 'Positions[x].Delta'. Looks like the validation framework is getting confused then.
        /// 
        /// </summary>
        /// <param name="entry"></param>
        private void ValidateDeltaOfPositions(CashJournalEntryViewModel entry) {
            CurrencyValidator validator = new CurrencyValidator();

            // fetch all form keys that end with 'Delta'
            var keys = HttpContext.Request.Form.AllKeys.Where(k => k.EndsWith("Delta"));
            foreach (var key in keys) {
                // fetch value of form field
                var val = HttpContext.Request.Form[key];
                // check if value is valid
                if (validator.IsValid(val))
                    continue;

                // now we know, that the value is not valid
                if (!ModelState.ContainsKey(key))
                    throw new Exception("CashJournalEntryPosition value for Delta is invalid, bu no reference can be found in ModelState!");

                // so we search the modelstate for the unlocalizable error message ...
                ModelError error = null;
                foreach (var err in ModelState[key].Errors) {
                    if (err.ErrorMessage.StartsWith("The value '") && err.ErrorMessage.EndsWith("' is not valid for Delta.")) {
                        error = err;
                        break;
                    }
                }

                // ... and remove it
                if (error != null)
                    ModelState[key].Errors.Remove(error);

                // if there are other errors, there is no need to add this error, and we can return directly
                if (ModelState[key].Errors.Count > 0)
                    return;

                var resourceManager = new ResourceManager(typeof(Resources.Localization));
                var resString = resourceManager.GetString("V_InvalidCurrencyFormat");

                // add the error for delta of the position to the model state
                ModelState.AddModelError(key, resString);
            }
        }
    }

    public class CurrencyValidator {
        private readonly Regex regEx = new Regex(Resources.Localization.CurrencyPattern, RegexOptions.Compiled);

        internal bool IsValid(string delta) {
            var match = regEx.Match(delta);
            return match.Success;
        }
    }
}
