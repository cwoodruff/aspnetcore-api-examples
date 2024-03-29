using Chinook.Domain.ApiModels;
using Chinook.Domain.Extensions;
using FluentValidation;
using Microsoft.Extensions.Caching.Memory;

namespace Chinook.Domain.Supervisor;

public partial class ChinookSupervisor
{
    public async Task<PagedList<InvoiceLineApiModel>> GetAllInvoiceLine(int pageNumber, int pageSize)
    {
        var invoiceLines = await invoiceLineRepository!.GetAll(pageNumber, pageSize);
        var invoiceLineApiModels = invoiceLines.ConvertAll().ToList();

        var lineApiModels = invoiceLineApiModels.ToList();
        foreach (var invoiceLine in lineApiModels)
        {
            var cacheEntryOptions =
                new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(604800))
                    .AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(604800);

            memoryCache!.Set(string.Concat("InvoiceLine-", invoiceLine.Id), invoiceLine, (TimeSpan)cacheEntryOptions);
        }

        var newPagedList = new PagedList<InvoiceLineApiModel>(lineApiModels, invoiceLines.TotalCount,
            invoiceLines.CurrentPage, invoiceLines.PageSize);
        return newPagedList;
    }

    public async Task<InvoiceLineApiModel> GetInvoiceLineById(int id)
    {
        var invoiceLineApiModelCached = memoryCache!.Get<InvoiceLineApiModel>(string.Concat("InvoiceLine-", id));

        if (invoiceLineApiModelCached != null)
        {
            return invoiceLineApiModelCached;
        }
        else
        {
            var invoiceLine = await invoiceLineRepository!.GetById(id);
            if (invoiceLine == null) return null!;
            var invoiceLineApiModel = invoiceLine.Convert();
            invoiceLineApiModel.Track = await GetTrackById(invoiceLineApiModel.TrackId);
            invoiceLineApiModel.Invoice = await GetInvoiceById(invoiceLineApiModel.InvoiceId);
            if (invoiceLineApiModel.Track != null) invoiceLineApiModel.TrackName = invoiceLineApiModel.Track.Name;

            var cacheEntryOptions =
                new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(604800))
                    .AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(604800);

            memoryCache!.Set(string.Concat("InvoiceLine-", invoiceLineApiModel.Id), invoiceLineApiModel,
                (TimeSpan)cacheEntryOptions);

            return invoiceLineApiModel;
        }
    }

    public async Task<PagedList<InvoiceLineApiModel>> GetInvoiceLineByInvoiceId(int id, int pageNumber, int pageSize)
    {
        var invoiceLines = await invoiceLineRepository!.GetByInvoiceId(id, pageNumber, pageSize);
        var invoiceLineApiModels = invoiceLines.ConvertAll();
        var newPagedList = new PagedList<InvoiceLineApiModel>(invoiceLineApiModels.ToList(), invoiceLines.TotalCount,
            invoiceLines.CurrentPage, invoiceLines.PageSize);
        return newPagedList;
    }

    public async Task<PagedList<InvoiceLineApiModel>> GetInvoiceLineByTrackId(int id, int pageNumber, int pageSize)
    {
        var invoiceLines = await invoiceLineRepository!.GetByTrackId(id, pageNumber, pageSize);
        var invoiceLineApiModels = invoiceLines.ConvertAll();
        var newPagedList = new PagedList<InvoiceLineApiModel>(invoiceLineApiModels.ToList(), invoiceLines.TotalCount,
            invoiceLines.CurrentPage, invoiceLines.PageSize);
        return newPagedList;
    }

    public async Task<InvoiceLineApiModel> AddInvoiceLine(InvoiceLineApiModel newInvoiceLineApiModel)
    {
        await invoiceLineValidator.ValidateAndThrowAsync(newInvoiceLineApiModel);

        var invoiceLine = newInvoiceLineApiModel.Convert();

        invoiceLine = await invoiceLineRepository!.Add(invoiceLine);
        newInvoiceLineApiModel.Id = invoiceLine.Id;
        return newInvoiceLineApiModel;
    }

    public async Task<bool> UpdateInvoiceLine(InvoiceLineApiModel invoiceLineApiModel)
    {
        await invoiceLineValidator.ValidateAndThrowAsync(invoiceLineApiModel);

        var invoiceLine = await invoiceLineRepository!.GetById(invoiceLineApiModel.InvoiceId);

        if (invoiceLine == null) return false;

        invoiceLine.InvoiceId = invoiceLineApiModel.InvoiceId;
        invoiceLine.TrackId = invoiceLineApiModel.TrackId;
        invoiceLine.UnitPrice = invoiceLineApiModel.UnitPrice;
        invoiceLine.Quantity = invoiceLineApiModel.Quantity;

        return await invoiceLineRepository.Update(invoiceLine);
    }

    public Task<bool> DeleteInvoiceLine(int id)
        => invoiceLineRepository!.Delete(id);
}