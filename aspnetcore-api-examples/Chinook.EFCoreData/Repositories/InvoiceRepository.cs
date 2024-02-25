using Chinook.Domain.Entities;
using Chinook.Domain.Extensions;
using Chinook.Domain.Repositories;
using Chinook.EFCoreData.Data;
using Microsoft.EntityFrameworkCore;

namespace Chinook.EFCoreData.Repositories;

public class InvoiceRepository(ChinookContext context) : BaseRepository<Invoice>(context), IInvoiceRepository
{
    public async Task<PagedList<Invoice>> GetByEmployeeId(int? id, int pageNumber, int pageSize) =>
        await PagedList<Invoice>.ToPagedListAsync(_context.Customers.Where(a => a.SupportRepId == id)
                .SelectMany(t => t.Invoices!)
                .AsNoTrackingWithIdentityResolution(),
            pageNumber,
            pageSize);

    public async Task<PagedList<Invoice>> GetByCustomerId(int? id, int pageNumber, int pageSize) =>
        await PagedList<Invoice>.ToPagedListAsync(_context.Invoices.Where(a => a.CustomerId == id)
                .AsNoTrackingWithIdentityResolution(),
            pageNumber,
            pageSize);
}