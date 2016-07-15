namespace CrossNtErp.DataAccess.Migrations {
    using Shared.Entities.Common;
    using Shared.Entities.Finances;
    using System;
    using System.Collections.ObjectModel;
    using System.Data.Entity.Migrations;
    using System.Diagnostics;
    using System.Linq;


    internal sealed class Configuration : DbMigrationsConfiguration<DataContext> {
        public Configuration() {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(DataContext context) {
            if (context.TaxRates.Count() >= 2) {
                Debug.WriteLine($"No need to seed ...");
                return;
            }

            Debug.WriteLine(" > Seeding data into database ...");
            Debug.Write($"     > ... Currencies");
            SeedCurrencies(context);
            Debug.WriteLine($" ... done!");
            Debug.Write($"     > ... Tax Rates");
            SeedTaxRates(context);
            Debug.WriteLine($" ... done!");
            Debug.Write($"     > ... Cash Journals");
            SeedJournals(context);
            Debug.WriteLine($" ... done!");
            Debug.Write($"     > ... Cash Journal Entries");
            SeedJournalEntries(context);
            Debug.WriteLine($" ... done!");
        }

        private void SeedCurrencies(DataContext context) {
            context.Currencies.AddOrUpdate(c => c.CultureName, new Currency() {
                CultureName = "de-DE"
            });

            context.Currencies.AddOrUpdate(c => c.CultureName, new Currency() {
                CultureName = "en-US"
            });

            context.Currencies.AddOrUpdate(c => c.CultureName, new Currency() {
                CultureName = "en-GB"
            });
        }

        private void SeedTaxRates(DataContext context) {
            context.TaxRates.AddOrUpdate(t => t.Category, new TaxRate() {
                Category = "VAT",
                Description = "Value added tax",
                Value = 19.00m,
                Id = 0
            });

            context.TaxRates.AddOrUpdate(t => t.Category, new TaxRate() {
                Category = "Reduced VAT",
                Description = "Reduced Value added tax",
                Value = 7.00m,
                Id = 0
            });
        }

        private void SeedJournals(DataContext context) {
            context.CashJournals.AddOrUpdate(j => j.Title, new CashJournal() {
                Title = "08/2016",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(30),
                Description = "August 2016",
                UserId = 2,
                DefaultCurrencyId = 1
            });
        }

        private void SeedJournalEntries(DataContext context) {
            CashJournal journal = context.CashJournals.FirstOrDefault();
            if (journal == null)
                journal = context.CashJournals.Local.First();

            CashJournalEntryPosition position = new CashJournalEntryPosition() {
                Delta = -200.0m,
                Description = "Devel Test",
                TaxRate = context.TaxRates.Local.First()
            };

            CashJournalEntry entry = new CashJournalEntry() {
                Date = DateTime.Now,
                ProcessDescription = "Some business process",
                Journal = journal,
                Positions = new ObservableCollection<CashJournalEntryPosition>(new CashJournalEntryPosition[] { position })
            };

            context.CashJournalEntries.AddOrUpdate(e => e.ProcessDescription, entry);
        }
    }
}
