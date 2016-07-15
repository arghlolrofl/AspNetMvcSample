using CrossNtErp.DataAccess;
using CrossNtErp.Web.Controllers.Base;
using CrossNtErp.Web.Models;
using CrossNtErp.Web.Security;
using System.Net;
using System.Web.Mvc;

namespace CrossNtErp.Web.Areas.Finances.Controllers {
    [AppAuthorize(AccessRole.CashJournal)]
    public class CashJournalEntryPositionsController : LocalizationControllerBase {
        private DataContext db = new DataContext();

        // GET: CashJournalEntryPositions
        public ActionResult Delete(long positionId, long entryId) {
            var position = db.CashJournalEntryPositions.Find(positionId);
            if (position == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            db.Entry(position).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();

            return RedirectToAction("Edit", "CashJournalEntries", new { id = entryId, area = "Finances" });
        }
    }
}