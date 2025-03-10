using Microsoft.AspNetCore.Mvc;
using Serilog;
using UKParliament.CodeTest.Services;
using UKParliament.CodeTest.Web.Contracts.Requests;
using UKParliament.CodeTest.Web.Contracts.Responses;
using UKParliament.CodeTest.Web.Mapping;
using ILogger = Serilog.ILogger;

namespace UKParliament.CodeTest.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonController : ControllerBase
{
    private readonly IPersonService _service;
    private readonly ILogger _logger;

    public PersonController(IPersonService service)
    {
        _service = service;
        _logger = Log.ForContext<PersonController>();
    }

    /// <summary>
    /// Get a specific person by ID
    /// </summary>
    /// <param name="id">Person ID</param>
    /// <returns>200 OK or 404 Not Found</returns>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<PersonResponse>> GetById(int id)
    {
        _logger.Information("Fetching person with ID {PersonId}", id);
        var result = await _service.GetPersonByIdAsync(id);
        if (!result.IsSuccess)
        {
            _logger.Warning("Person with ID {PersonId} not found.", id);
            return NotFound(new { message = result.ErrorMessage });
        }

        _logger.Information("Successfully fetched person with ID {PersonId}", id);
        return Ok(result.Data!.MapToResponse());
    }

    /// <summary>
    /// Get all people
    /// </summary>
    /// <returns>200 OK or 400 Bad Request</returns>
    [HttpGet]
    public async Task<ActionResult<PersonsResponse>> GetAll()
    {
        _logger.Information("Fetching all active people.");
        var result = await _service.GetAllPeopleAsync();
        if (!result.IsSuccess)
        {
            _logger.Warning("Failed to fetch all people: {ErrorMessage}", result.ErrorMessage);
            return BadRequest(new { message = result.ErrorMessage });
        }

        _logger.Information("Successfully fetched {Count} people.", result.Data!.Count);
        return Ok(result.Data!.MapToResponse());
    }

    /// <summary>
    /// Create a new person
    /// </summary>
    /// <param name="request">Person details</param>
    /// <returns>201 Created or 400 Bad Request</returns>
    [HttpPost]
    public async Task<ActionResult<PersonResponse>> CreatePerson([FromBody] CreatePersonRequest request)
    {
        _logger.Information("Creating new person: {@Request}", request);
        var person = request.MapToPerson();
        var result = await _service.AddPersonAsync(person);

        if (!result.IsSuccess)
        {
            _logger.Warning("Failed to create person: {ErrorMessage}", result.ErrorMessage);
            return BadRequest(new { message = result.ErrorMessage });
        }

        _logger.Information("Person created successfully: {@Person}", person);
        return CreatedAtAction(nameof(GetById), new { id = person.Id }, person.MapToResponse());
    }

    /// <summary>
    /// Update an existing person
    /// </summary>
    /// <param name="id">Person ID</param>
    /// <param name="request">Updated person details</param>
    /// <returns>200 OK, 400 Bad Request, or 404 Not Found</returns>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<PersonResponse>> UpdatePerson(int id, [FromBody] UpdatePersonRequest request)
    {
        _logger.Information("Updating person with ID {PersonId}: {@Request}", id, request);
        var existingPerson = await _service.GetPersonByIdAsync(id);
        if (!existingPerson.IsSuccess)
        {
            _logger.Warning("Person with ID {PersonId} not found.", id);
            return NotFound(new { message = existingPerson.ErrorMessage });
        }

        var person = request.MapToPerson(id);
        var result = await _service.UpdatePersonAsync(person);

        if (!result.IsSuccess)
        {
            _logger.Warning("Failed to update person with ID {PersonId}: {ErrorMessage}", id, result.ErrorMessage);
            return BadRequest(new { message = result.ErrorMessage });
        }

        _logger.Information("Person with ID {PersonId} updated successfully.", id);
        return Ok(person.MapToResponse());
    }

    /// <summary>
    /// Soft-delete a person (deactivate)
    /// </summary>
    /// <param name="id">Person ID</param>
    /// <returns>200 OK, 400 Bad Request, or 404 Not Found</returns>
    [HttpPatch("{id}/deactivate")]
    public async Task<IActionResult> DeactivatePerson(int id)
    {
        _logger.Information("Deactivating person with ID {PersonId}", id);
        var result = await _service.DeactivatePersonAsync(id);
        if (!result.IsSuccess)
        {
            _logger.Warning("Failed to deactivate person with ID {PersonId}: {ErrorMessage}", id, result.ErrorMessage);
            if (result.ErrorMessage == "Person not found.")
                return NotFound(new { message = result.ErrorMessage });
            return BadRequest(new { message = result.ErrorMessage });
        }

        _logger.Information("Person with ID {PersonId} deactivated successfully", id);
        return Ok(new { message = "Person has been deactivated." });
    }

    /// <summary>
    /// Reactivate a person
    /// </summary>
    /// <param name="id">Person ID</param>
    /// <returns>200 OK, 400 Bad Request, or 404 Not Found</returns>
    [HttpPatch("{id}/activate")]
    public async Task<IActionResult> ActivatePerson(int id)
    {
        _logger.Information("Activating person with ID {PersonId}", id);
        var result = await _service.ActivatePersonAsync(id);
        if (!result.IsSuccess)
        {
            _logger.Warning("Failed to activate person with ID {PersonId}: {ErrorMessage}", id, result.ErrorMessage);
            if (result.ErrorMessage == "Person not found.")
                return NotFound(new { message = result.ErrorMessage });
            return BadRequest(new { message = result.ErrorMessage });
        }

        _logger.Information("Person with ID {PersonId} activated successfully", id);
        return Ok(new { message = "Person has been reactivated." });
    }

    /// <summary>
    /// Permanently delete a person
    /// </summary>
    /// <param name="id">Person ID</param>
    /// <returns>200 OK or 404 Not Found</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePerson(int id)
    {
        _logger.Information("Deleting person with ID {PersonId}", id);
        var result = await _service.DeletePersonAsync(id);
        if (!result.IsSuccess)
        {
            _logger.Warning("Failed to delete person with ID {PersonId}: {ErrorMessage}", id, result.ErrorMessage);
            return NotFound(new { message = result.ErrorMessage });
        }

        _logger.Information("Person with ID {PersonId} deleted successfully.", id);
        return Ok(new { message = "Person has been permanently deleted." });
    }
}