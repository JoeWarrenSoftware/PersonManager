using Microsoft.AspNetCore.Mvc;
using Serilog;
using UKParliament.CodeTest.Services;
using UKParliament.CodeTest.Web.Contracts.Responses;
using UKParliament.CodeTest.Web.Mapping;
using ILogger = Serilog.ILogger;

namespace UKParliament.CodeTest.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepartmentController : ControllerBase
{
    private readonly IDepartmentService _service;
    private readonly ILogger<DepartmentController> _logger;

    public DepartmentController(IDepartmentService service, ILogger<DepartmentController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all departments
    /// </summary>
    /// <returns>200 OK or 400 Bad Request</returns>
    [HttpGet]
    public async Task<ActionResult<DepartmentsResponse>> GetAll()
    {
        _logger.LogInformation("Fetching all departments.");
        var result = await _service.GetAllDepartmentsAsync();
        if (!result.IsSuccess)
        {
            _logger.LogWarning("Failed to fetch all departments: {ErrorMessage}", result.ErrorMessage);
            return BadRequest(new { message = result.ErrorMessage });
        }

        _logger.LogInformation("Successfully fetched {Count} departments.", result.Data!.Count);
        return Ok(result.Data!.MapToResponse());
    }
}
