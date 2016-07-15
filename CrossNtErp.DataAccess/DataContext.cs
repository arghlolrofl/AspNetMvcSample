using CrossNtErp.Shared.Entities.Common;
using CrossNtErp.Shared.Entities.Finances;
using System.Data.Entity;

namespace CrossNtErp.DataAccess {
    public class DataContext : DbContext {
        const string ConnectionStringName = "DbConnection";

        #region Context Entities

        public DbSet<CashJournal> CashJournals { get; set; }
        public DbSet<CashJournalEntry> CashJournalEntries { get; set; }
        public DbSet<CashJournalEntryPosition> CashJournalEntryPositions { get; set; }
        public DbSet<TaxRate> TaxRates { get; set; }
        public DbSet<Currency> Currencies { get; set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the database context class.
        /// </summary>
        public DataContext() : base(ConnectionStringName) {

        }

        /// <summary>
        /// Configures entities when building the databse model from POCO's.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
        }
    }
}
