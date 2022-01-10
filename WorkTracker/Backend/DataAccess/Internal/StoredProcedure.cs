namespace DataAccess.Internal;

public enum StoredProcedure
{
    sp_Invalid,

    // Generics
    spGeneric_GetAll,

    // Employer
    spEmployer_Create,
    spEmployer_Update,
    spEmployer_Delete,
    spEmployer_GetBalance,

    // WorkEntry
    spWorkEntry_Create,
    spWorkEntry_Update,
    spWorkEntry_Delete,
    spWorkEntry_GetAllByEmployerId,
    spWorkEntry_GetLatest,

    // Invoice
    spInvoice_Create,
    spInvoice_Update,
    spInvoice_Delete,
    spInvoice_GetBetween,
    spInvoice_GetCreatedBetween,
}
