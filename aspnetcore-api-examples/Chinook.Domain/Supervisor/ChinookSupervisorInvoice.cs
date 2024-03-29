using Chinook.Domain.ApiModels;
using Chinook.Domain.Extensions;
using FluentValidation;
using Microsoft.Extensions.Caching.Memory;

namespace Chinook.Domain.Supervisor;

public partial class ChinookSupervisor
{
    public async Task<PagedList<InvoiceApiModel>> GetAllInvoice(int pageNumber, int pageSize)
    {
        var invoices = await invoiceRepository!.GetAll(pageNumber, pageSize);
        var invoiceApiModels = invoices.ConvertAll().ToList();

        foreach (var invoice in invoiceApiModels)
        {
            var cacheEntryOptions =
                new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(604800))
                    .AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(604800);

            memoryCache!.Set(string.Concat("Invoice-", invoice.Id), invoice, (TimeSpan)cacheEntryOptions);
        }

        var newPagedList = new PagedList<InvoiceApiModel>(invoiceApiModels, invoices.TotalCount, invoices.CurrentPage,
            invoices.PageSize);
        return newPagedList;
    }

    public async Task<InvoiceApiModel?> GetInvoiceById(int? id)
    {
        var invoiceApiModelCached = memoryCache!.Get<InvoiceApiModel>(string.Concat("Invoice-", id));

        if (invoiceApiModelCached != null)
        {
            return invoiceApiModelCached;
        }
        else
        {
            var invoice = await invoiceRepository!.GetById(id);
            if (invoice == null) return null;
            var invoiceApiModel = invoice.Convert();
            //invoiceApiModel.InvoiceLines = (await GetInvoiceLineByInvoiceId(invoiceApiModel.Id)).ToList();

            var cacheEntryOptions =
                new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(604800))
                    .AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(604800);

            memoryCache!.Set(string.Concat("Invoice-", invoiceApiModel.Id), invoiceApiModel, (TimeSpan)cacheEntryOptions);

            return invoiceApiModel;
        }
    }

    public async Task<PagedList<InvoiceApiModel>> GetInvoiceByCustomerId(int id, int pageNumber, int pageSize)
    {
        var invoices = await invoiceRepository!.GetByCustomerId(id, pageNumber, pageSize);
        var invoiceApiModels = invoices.ConvertAll();
        var newPagedList = new PagedList<InvoiceApiModel>(invoiceApiModels.ToList(), invoices.TotalCount,
            invoices.CurrentPage, invoices.PageSize);
        return newPagedList;
    }

    public async Task<InvoiceApiModel> AddInvoice(InvoiceApiModel newInvoiceApiModel)
    {
        await invoiceValidator.ValidateAndThrowAsync(newInvoiceApiModel);

        var invoice = newInvoiceApiModel.Convert();

        invoice = await invoiceRepository!.Add(invoice);
        newInvoiceApiModel.Id = invoice.Id;
        return newInvoiceApiModel;
    }

    public async Task<bool> UpdateInvoice(InvoiceApiModel invoiceApiModel)
    {
        await invoiceValidator.ValidateAndThrowAsync(invoiceApiModel);

        var invoice = await invoiceRepository!.GetById(invoiceApiModel.Id);

        if (invoice == null) return false;

        invoice.CustomerId = invoiceApiModel.CustomerId;
        invoice.InvoiceDate = invoiceApiModel.InvoiceDate;
        invoice.BillingAddress = invoiceApiModel.BillingAddress ?? string.Empty;
        invoice.BillingCity = invoiceApiModel.BillingCity ?? string.Empty;
        invoice.BillingState = invoiceApiModel.BillingState ?? string.Empty;
        invoice.BillingCountry = invoiceApiModel.BillingCountry ?? string.Empty;
        invoice.BillingPostalCode = invoiceApiModel.BillingPostalCode ?? string.Empty;
        invoice.Total = invoiceApiModel.Total;

        return await invoiceRepository.Update(invoice);
    }

    public Task<bool> DeleteInvoice(int id)
        => invoiceRepository!.Delete(id);


    public async Task<PagedList<InvoiceApiModel>> GetInvoiceByEmployeeId(int id, int pageNumber, int pageSize)
    {
        var invoices = await invoiceRepository!.GetByEmployeeId(id, pageNumber, pageSize);
        var invoiceApiModels = invoices.ConvertAll();
        var newPagedList = new PagedList<InvoiceApiModel>(invoiceApiModels.ToList(), invoices.TotalCount,
            invoices.CurrentPage, invoices.PageSize);
        return newPagedList;
    }
}