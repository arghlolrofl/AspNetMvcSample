using System;

namespace CrossNtErp.Web.Models {
    [Flags]
    public enum AccessRole {
        /* Default user role */
        User = 0,

        /* Admin user role and sub roles */
        Admin = 1,

        /* Finances user role and sub roles */
        Finances = 2,

        CashJournal = 4,
        TaxRate = 8
    }
}