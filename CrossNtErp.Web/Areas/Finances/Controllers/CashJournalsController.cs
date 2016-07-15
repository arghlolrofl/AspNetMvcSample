using CrossNtErp.DataAccess;
using CrossNtErp.Shared.Entities.Finances;
using CrossNtErp.Web.Areas.Finances.Models;
using CrossNtErp.Web.Controllers.Base;
using CrossNtErp.Web.Models;
using CrossNtErp.Web.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace CrossNtErp.Web.Areas.Finances.Controllers {
    [AppAuthorize(AccessRole.CashJournal | AccessRole.Admin)]
    public class CashJournalsController : LocalizationControllerBase {
        private DataContext db = new DataContext();


        /// <summary>
        /// GET: Finances/CashJournals
        /// </summary>
        /// <returns></returns>
        public ActionResult Index() {
            List<CashJournalViewModel> viewModels = new List<CashJournalViewModel>();

            var userId = User.GetId();
            var journals = db.CashJournals.Where(cj => cj.UserId == userId || cj.UserId <= 0).ToList();

            foreach (CashJournal journal in journals)
                viewModels.Add(new CashJournalViewModel(journal));

            return View(viewModels);
        }

        /// <summary>
        /// GET: Finances/CashJournals/Details/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(long? id) {
            if (!id.HasValue || id.Value <= 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            CashJournal cashJournal = db.CashJournals.Find(id);
            if (cashJournal == null)
                return HttpNotFound();

            if (!CheckUserAuthorization(cashJournal))
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            return View(new CashJournalViewModel(cashJournal));
        }

        /// <summary>
        /// GET: Finances/CashJournals/Create
        /// </summary>
        /// <returns></returns>
        public ActionResult Create() {
            CashJournalViewModel vm = new CashJournalViewModel() {
                UserId = User.GetId(),
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(1)
            };

            return View(vm);
        }

        // POST: Finances/CashJournals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Title,StartDate,EndDate,Description,UserId")] CashJournalViewModel cashJournal) {
            if (ModelState.IsValid) {
                CashJournal newJournal = new CashJournal() {
                    Title = cashJournal.Title,
                    StartDate = cashJournal.StartDate,
                    EndDate = cashJournal.EndDate,
                    Description = cashJournal.Description,
                    UserId = cashJournal.UserId,
                };
                db.CashJournals.Add(newJournal);
                db.SaveChanges();
                return RedirectToAction("Edit", new { id = newJournal.Id });
            }

            return View(cashJournal);
        }

        /// <summary>
        /// GET: Finances/CashJournals/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(long? id) {
            if (!id.HasValue || id.Value <= 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            CashJournal cashJournal = db.CashJournals
                                        .Include(cj => cj.Entries)
                                        .SingleOrDefault(cj => cj.Id == id);

            if (cashJournal == null)
                return HttpNotFound();

            return View(new CashJournalViewModel(cashJournal));
        }

        // POST: Finances/CashJournals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,StartDate,EndDate,Description,UserId")] CashJournalViewModel cashJournal) {
            if (ModelState.IsValid) {
                CashJournal journalToUpdate = db.CashJournals.Find(cashJournal.Id);
                if (journalToUpdate == null)
                    return HttpNotFound();

                journalToUpdate.Title = cashJournal.Title;
                journalToUpdate.StartDate = cashJournal.StartDate;
                journalToUpdate.EndDate = cashJournal.EndDate;
                journalToUpdate.Description = cashJournal.Description;
                journalToUpdate.UserId = cashJournal.UserId;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(cashJournal);
        }

        /// <summary>
        /// GET: Finances/CashJournals/Delete/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(long? id) {
            if (!id.HasValue || id.Value <= 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            CashJournal cashJournal = db.CashJournals.Find(id);
            if (cashJournal == null)
                return HttpNotFound();

            return View(new CashJournalViewModel(cashJournal));
        }

        /// <summary>
        /// POST: Finances/CashJournals/Delete/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id) {
            CashJournal cashJournal = db.CashJournals.Find(id);
            if (cashJournal == null)
                return HttpNotFound();

            db.CashJournals.Remove(cashJournal);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CheckUserAuthorization(CashJournal journal) => journal.UserId == User.GetId();
    }
}
