using Chinook.Domain.Entities;
using Chinook.Domain.Extensions;
using Chinook.Domain.Repositories;
using Chinook.EFCoreData.Data;
using Microsoft.EntityFrameworkCore;

namespace Chinook.EFCoreData.Repositories;

public class CustomerRepository(ChinookContext context) : BaseRepository<Customer>(context), ICustomerRepository
{
    public async Task<PagedList<Customer>> GetBySupportRepId(int? id, int pageNumber, int pageSize) =>
        await PagedList<Customer>.ToPagedListAsync(_context.Customers.Where(a => a.SupportRepId == id)
                .AsNoTrackingWithIdentityResolution(),
            pageNumber,
            pageSize);
}