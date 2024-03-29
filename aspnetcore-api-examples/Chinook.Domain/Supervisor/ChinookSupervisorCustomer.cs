using Chinook.Domain.ApiModels;
using Chinook.Domain.Extensions;
using FluentValidation;
using Microsoft.Extensions.Caching.Memory;

namespace Chinook.Domain.Supervisor;

public partial class ChinookSupervisor
{
    public async Task<PagedList<CustomerApiModel>> GetAllCustomer(int pageNumber, int pageSize)
    {
        var customers = await customerRepository!.GetAll(pageNumber, pageSize);
        var customerApiModels = customers.ConvertAll().ToList();

        foreach (var customer in customerApiModels)
        {
            var cacheEntryOptions =
                new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(604800))
                    .AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(604800);
            memoryCache!.Set(string.Concat("Customer-", customer.Id), customer, (TimeSpan)cacheEntryOptions);
        }

        var newPagedList = new PagedList<CustomerApiModel>(customerApiModels, customers.TotalCount,
            customers.CurrentPage, customers.PageSize);
        return newPagedList;
    }

    public async Task<CustomerApiModel> GetCustomerById(int id)
    {
        var customerApiModelCached = memoryCache!.Get<CustomerApiModel>(string.Concat("Customer-", id));

        if (customerApiModelCached != null)
        {
            return customerApiModelCached;
        }
        else
        {
            var customer = await customerRepository!.GetById(id);
            if (customer == null) return null!;
            var customerApiModel = customer.Convert();
            //customerApiModel.Invoices = (await GetInvoiceByCustomerId(customerApiModel.Id)).ToList();
            customerApiModel.SupportRep =
                await GetEmployeeById(customerApiModel.SupportRepId);
            if (customerApiModel.SupportRep != null)
                customerApiModel.SupportRepName =
                    $"{customerApiModel.SupportRep.LastName}, {customerApiModel.SupportRep.FirstName}";

            var cacheEntryOptions =
                new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(604800))
                    .AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(604800);
            memoryCache!.Set(string.Concat("Customer-", customerApiModel.Id), customerApiModel,
                (TimeSpan)cacheEntryOptions);

            return customerApiModel;
        }
    }

    public async Task<PagedList<CustomerApiModel>> GetCustomerBySupportRepId(int id, int pageNumber, int pageSize)
    {
        var customers = await customerRepository!.GetBySupportRepId(id, pageNumber, pageSize);
        var customerApiModels = customers.ConvertAll();

        foreach (var customer in customers)
        {
            var cacheEntryOptions =
                new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(604800))
                    .AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(604800);
            memoryCache!.Set(string.Concat("Customer-", customer.Id), customer, (TimeSpan)cacheEntryOptions);
        }

        var newPagedList = new PagedList<CustomerApiModel>(customerApiModels.ToList(), customers.TotalCount,
            customers.CurrentPage, customers.PageSize);
        return newPagedList;
    }

    public async Task<CustomerApiModel> AddCustomer(CustomerApiModel newCustomerApiModel)
    {
        await customerValidator.ValidateAndThrowAsync(newCustomerApiModel);

        var customer = newCustomerApiModel.Convert();

        customer = await customerRepository!.Add(customer);
        newCustomerApiModel.Id = customer.Id;
        return newCustomerApiModel;
    }

    public async Task<bool> UpdateCustomer(CustomerApiModel customerApiModel)
    {
        await customerValidator.ValidateAndThrowAsync(customerApiModel);

        var customer = await customerRepository!.GetById(customerApiModel.Id);

        if (customer == null) return false;
        customer.FirstName = customerApiModel.FirstName;
        customer.LastName = customerApiModel.LastName;
        customer.Company = customerApiModel.Company ?? string.Empty;
        customer.Address = customerApiModel.Address ?? string.Empty;
        customer.City = customerApiModel.City ?? string.Empty;
        customer.State = customerApiModel.State ?? string.Empty;
        customer.Country = customerApiModel.Country ?? string.Empty;
        customer.PostalCode = customerApiModel.PostalCode ?? string.Empty;
        customer.Phone = customerApiModel.Phone ?? string.Empty;
        customer.Fax = customerApiModel.Fax ?? string.Empty;
        customer.Email = customerApiModel.Email ?? string.Empty;
        customer.SupportRepId = customerApiModel.SupportRepId;

        return await customerRepository.Update(customer);
    }

    public Task<bool> DeleteCustomer(int id)
        => customerRepository!.Delete(id);
}