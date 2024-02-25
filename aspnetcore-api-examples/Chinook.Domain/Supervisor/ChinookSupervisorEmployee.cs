using Chinook.Domain.ApiModels;
using Chinook.Domain.Extensions;
using FluentValidation;
using Microsoft.Extensions.Caching.Memory;

namespace Chinook.Domain.Supervisor;

public partial class ChinookSupervisor
{
    public async Task<PagedList<EmployeeApiModel>> GetAllEmployee(int pageNumber, int pageSize)
    {
        var employees = await employeeRepository!.GetAll(pageNumber, pageSize);
        var employeeApiModels = employees.ConvertAll().ToList();

        foreach (var employee in employeeApiModels)
        {
            var cacheEntryOptions =
                new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(604800))
                    .AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(604800);

            memoryCache!.Set(string.Concat("Employee-", employee.Id), employee, (TimeSpan)cacheEntryOptions);
        }

        var newPagedList = new PagedList<EmployeeApiModel>(employeeApiModels, employees.TotalCount,
            employees.CurrentPage, employees.PageSize);
        return newPagedList;
    }

    public async Task<EmployeeApiModel?> GetEmployeeById(int? id)
    {
        var employeeApiModelCached = memoryCache!.Get<EmployeeApiModel>(string.Concat("Employee-", id));

        if (employeeApiModelCached != null)
        {
            return employeeApiModelCached;
        }
        else
        {
            var employee = await employeeRepository!.GetById(id);
            if (employee == null) return null;
            var employeeApiModel = employee.Convert();

            var cacheEntryOptions =
                new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(604800))
                    .AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(604800);

            memoryCache!.Set(string.Concat("Employee-", employeeApiModel.Id), employeeApiModel,
                (TimeSpan)cacheEntryOptions);

            return employeeApiModel;
        }
    }

    public async Task<EmployeeApiModel?> GetEmployeeReportsTo(int id)
    {
        var employee = await employeeRepository!.GetReportsTo(id);
        return employee.Convert();
    }

    public async Task<EmployeeApiModel> AddEmployee(EmployeeApiModel newEmployeeApiModel)
    {
        await employeeValidator.ValidateAndThrowAsync(newEmployeeApiModel);

        var employee = newEmployeeApiModel.Convert();

        employee = await employeeRepository!.Add(employee);
        newEmployeeApiModel.Id = employee.Id;
        return newEmployeeApiModel;
    }

    public async Task<bool> UpdateEmployee(EmployeeApiModel employeeApiModel)
    {
        await employeeValidator.ValidateAndThrowAsync(employeeApiModel);

        var employee = await employeeRepository!.GetById(employeeApiModel.Id);

        if (employee == null) return false;

        employee.LastName = employeeApiModel.LastName;
        employee.FirstName = employeeApiModel.FirstName;
        employee.Title = employeeApiModel.Title ?? string.Empty;
        employee.ReportsTo = employeeApiModel.ReportsTo;
        employee.BirthDate = employeeApiModel.BirthDate;
        employee.HireDate = employeeApiModel.HireDate;
        employee.Address = employeeApiModel.Address ?? string.Empty;
        employee.City = employeeApiModel.City ?? string.Empty;
        employee.State = employeeApiModel.State ?? string.Empty;
        employee.Country = employeeApiModel.Country ?? string.Empty;
        employee.PostalCode = employeeApiModel.PostalCode ?? string.Empty;
        employee.Phone = employeeApiModel.Phone ?? string.Empty;
        employee.Fax = employeeApiModel.Fax ?? string.Empty;
        employee.Email = employeeApiModel.Email ?? string.Empty;

        return await employeeRepository.Update(employee);
    }

    public Task<bool> DeleteEmployee(int id)
        => employeeRepository!.Delete(id);

    public async Task<IEnumerable<EmployeeApiModel>> GetEmployeeDirectReports(int id)
    {
        var employees = await employeeRepository!.GetDirectReports(id);
        return employees.ConvertAll();
    }

    public async Task<IEnumerable<EmployeeApiModel>> GetDirectReports(int id)
    {
        var employees = await employeeRepository!.GetDirectReports(id);
        return employees.ConvertAll();
    }
}