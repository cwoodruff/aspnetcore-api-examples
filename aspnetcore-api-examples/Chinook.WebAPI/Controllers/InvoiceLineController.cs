using System.Net;
using System.Text.Json;
using Chinook.Domain.ApiModels;
using Chinook.Domain.Extensions;
using Chinook.Domain.Supervisor;
using FluentValidation;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Chinook.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[EnableCors("CorsPolicy")]
[ApiVersion("1.0")]
public class InvoiceLineController(IChinookSupervisor chinookSupervisor, ILogger<InvoiceLineController> logger)
    : ControllerBase
{
    [HttpGet]
    [Produces("application/json")]
    public async Task<ActionResult<PagedList<InvoiceLineApiModel>>> Get([FromQuery] int pageNumber,
        [FromQuery] int pageSize)
    {
        try
        {
            var invoiceLines = await chinookSupervisor.GetAllInvoiceLine(pageNumber, pageSize);

            if (invoiceLines.Any())
            {
                var metadata = new
                {
                    invoiceLines.TotalCount,
                    invoiceLines.PageSize,
                    invoiceLines.CurrentPage,
                    invoiceLines.TotalPages,
                    invoiceLines.HasNext,
                    invoiceLines.HasPrevious
                };
                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metadata));
                return Ok(invoiceLines);
            }

            return StatusCode((int)HttpStatusCode.NotFound, "No InvoiceLines Could Be Found");
        }
        catch (Exception ex)
        {
            logger.LogError($"Something went wrong inside the InvoiceLineController Get action: {ex}");
            return StatusCode((int)HttpStatusCode.InternalServerError,
                "Error occurred while executing Get All InvoiceLines");
        }
    }

    [HttpGet("{id}", Name = "GetInvoiceLineById")]
    [Produces("application/json")]
    public async Task<ActionResult<InvoiceLineApiModel>> Get(int id)
    {
        try
        {
            var invoiceLine = await chinookSupervisor.GetInvoiceLineById(id);

            if (invoiceLine != null)
            {
                return Ok(invoiceLine);
            }

            return StatusCode((int)HttpStatusCode.NotFound, "InvoiceLine Not Found");
        }
        catch (Exception ex)
        {
            logger.LogError($"Something went wrong inside the InvoiceLineController GetById action: {ex}");
            return StatusCode((int)HttpStatusCode.InternalServerError,
                "Error occurred while executing Get InvoiceLine By Id");
        }
    }

    [HttpPost]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<ActionResult<InvoiceLineApiModel>> Post([FromBody] InvoiceLineApiModel input)
    {
        try
        {
            if (input == null)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, "Given InvoiceLine is null");
            }

            return Ok(await chinookSupervisor.AddInvoiceLine(input));
        }
        catch (ValidationException ex)
        {
            logger.LogError($"Something went wrong inside the InvoiceLineController Add InvoiceLine action: {ex}");
            return StatusCode((int)HttpStatusCode.InternalServerError,
                "Error occurred while executing Add InvoiceLines");
        }
        catch (Exception ex)
        {
            logger.LogError($"Something went wrong inside the InvoiceLineController Add InvoiceLine action: {ex}");
            return StatusCode((int)HttpStatusCode.InternalServerError,
                "Error occurred while executing Add InvoiceLines");
        }
    }

    [HttpPut("{id}")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<ActionResult<InvoiceLineApiModel>> Put(int id, [FromBody] InvoiceLineApiModel input)
    {
        try
        {
            if (input == null)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, "Given InvoiceLine is null");
            }

            return Ok(await chinookSupervisor.UpdateInvoiceLine(input));
        }
        catch (ValidationException ex)
        {
            logger.LogError(
                $"Something went wrong inside the InvoiceLineController Update InvoiceLine action: {ex}");
            return StatusCode((int)HttpStatusCode.InternalServerError,
                "Error occurred while executing Update InvoiceLines");
        }
        catch (Exception ex)
        {
            logger.LogError(
                $"Something went wrong inside the InvoiceLineController Update InvoiceLine action: {ex}");
            return StatusCode((int)HttpStatusCode.InternalServerError,
                "Error occurred while executing Update InvoiceLines");
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            return Ok(await chinookSupervisor.DeleteInvoiceLine(id));
        }
        catch (Exception ex)
        {
            logger.LogError($"Something went wrong inside the InvoiceLineController Delete action: {ex}");
            return StatusCode((int)HttpStatusCode.InternalServerError,
                "Error occurred while executing Delete InvoiceLine");
        }
    }

    [HttpGet("invoice/{id}")]
    [Produces("application/json")]
    public async Task<ActionResult<PagedList<InvoiceLineApiModel>>> GetByInvoiceId(int id, [FromQuery] int pageNumber,
        [FromQuery] int pageSize)
    {
        try
        {
            var invoiceLines = await chinookSupervisor.GetInvoiceLineByInvoiceId(id, pageNumber, pageSize);

            if (invoiceLines.Any())
            {
                var metadata = new
                {
                    invoiceLines.TotalCount,
                    invoiceLines.PageSize,
                    invoiceLines.CurrentPage,
                    invoiceLines.TotalPages,
                    invoiceLines.HasNext,
                    invoiceLines.HasPrevious
                };
                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metadata));
                return Ok(invoiceLines);
            }

            return StatusCode((int)HttpStatusCode.NotFound, "No InvoiceLines Could Be Found for the Invoice");
        }
        catch (Exception ex)
        {
            logger.LogError($"Something went wrong inside the InvoiceLineController GetByInvoiceId action: {ex}");
            return StatusCode((int)HttpStatusCode.InternalServerError,
                "Error occurred while executing Get All InvoiceLines for Invoice");
        }
    }

    [HttpGet("track/{id}")]
    [Produces("application/json")]
    public async Task<ActionResult<PagedList<InvoiceLineApiModel>>> GetByTrackId(int id, [FromQuery] int pageNumber,
        [FromQuery] int pageSize)
    {
        try
        {
            var invoiceLines = await chinookSupervisor.GetInvoiceLineByTrackId(id, pageNumber, pageSize);

            if (invoiceLines.Any())
            {
                var metadata = new
                {
                    invoiceLines.TotalCount,
                    invoiceLines.PageSize,
                    invoiceLines.CurrentPage,
                    invoiceLines.TotalPages,
                    invoiceLines.HasNext,
                    invoiceLines.HasPrevious
                };
                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metadata));
                return Ok(invoiceLines);
            }

            return StatusCode((int)HttpStatusCode.NotFound, "No InvoiceLines Could Be Found for the Track");
        }
        catch (Exception ex)
        {
            logger.LogError($"Something went wrong inside the InvoiceLineController Get By Track action: {ex}");
            return StatusCode((int)HttpStatusCode.InternalServerError,
                "Error occurred while executing Get All InvoiceLines for Track");
        }
    }
}