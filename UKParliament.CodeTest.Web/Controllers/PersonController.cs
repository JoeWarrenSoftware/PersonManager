using FluentValidation;
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
    private readonly ILogger<PersonController> _logger;
    private readonly IValidator<CreatePersonRequest> _createValidator;
    private readonly IValidator<UpdatePersonRequest> _updateValidator;

    private const string _serverErrorMessage = "An error occurred while processing your request. Please try again later.";

    public PersonController(
        IPersonService service,
        ILogger<PersonController> logger,
        IValidator<CreatePersonRequest> createValidator,
        IValidator<UpdatePersonRequest> updateValidator)
    {
        _service = service;
        _logger = logger;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    /// <summary>
    /// Get a specific person by ID
    /// </summary>
    /// <param name="id">Person ID</param>
    /// <returns>200 OK or 404 Not Found</returns>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<PersonResponse>> GetById(int id)
    {
        _logger.LogInformation("Fetching person with ID {PersonId}", id);
        var result = await _service.GetPersonByIdAsync(id);
        if (!result.IsSuccess)
        {
            _logger.LogWarning("Person with ID {PersonId} not found.", id);
            return NotFound(new { message = result.ErrorMessage });
        }

        _logger.LogInformation("Successfully fetched person with ID {PersonId}", id);
        return Ok(result.Data!.MapToResponse());
    }

    /// <summary>
    /// Get all people
    /// </summary>
    /// <returns>200 OK or 400 Bad Request</returns>
    [HttpGet]
    public async Task<ActionResult<PersonsResponse>> GetAll()
    {
        _logger.LogInformation("Fetching all active people.");
        var result = await _service.GetAllPeopleAsync();
        if (!result.IsSuccess)
        {
            _logger.LogWarning("Failed to fetch all people: {ErrorMessage}", result.ErrorMessage);
            return BadRequest(new { message = result.ErrorMessage });
        }

        _logger.LogInformation("Successfully fetched {Count} people.", result.Data!.Count);
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
        _logger.LogInformation("Validating create person request: {@Request}", request);

        var validationResult = await _createValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for create person request.");
            return BadRequest(new { errors = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }) });
        }

        var person = request.MapToPerson();
        var result = await _service.AddPersonAsync(person);

        if (!result.IsSuccess)
        {
            _logger.LogWarning("Failed to create person: {ErrorMessage}", result.ErrorMessage);
            return BadRequest(new { message = result.ErrorMessage });
        }

        _logger.LogInformation("Person created successfully: {@Person}", person);
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
        _logger.LogInformation("Validating update person request: {@Request}", request);

        var validationResult = await _updateValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for update person request.");
            return BadRequest(new { errors = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }) });
        }

        var existingPerson = await _service.GetPersonByIdAsync(id);
        if (!existingPerson.IsSuccess)
        {
            _logger.LogWarning("Person with ID {PersonId} not found.", id);
            return NotFound(new { message = existingPerson.ErrorMessage });
        }

        var person = request.MapToPerson(id);
        var result = await _service.UpdatePersonAsync(person);

        if (!result.IsSuccess)
        {
            _logger.LogWarning("Failed to update person with ID {PersonId}: {ErrorMessage}", id, result.ErrorMessage);
            return BadRequest(new { message = result.ErrorMessage });
        }

        _logger.LogInformation("Person with ID {PersonId} updated successfully.", id);
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
        _logger.LogInformation("Deactivating person with ID {PersonId}", id);
        var result = await _service.DeactivatePersonAsync(id);
        if (!result.IsSuccess)
        {
            _logger.LogWarning("Failed to deactivate person with ID {PersonId}: {ErrorMessage}", id, result.ErrorMessage);
            if (result.ErrorMessage == "Person not found.")
                return NotFound(new { message = result.ErrorMessage });
            return BadRequest(new { message = result.ErrorMessage });
        }

        _logger.LogInformation("Person with ID {PersonId} deactivated successfully", id);
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
        _logger.LogInformation("Activating person with ID {PersonId}", id);
        var result = await _service.ActivatePersonAsync(id);
        if (!result.IsSuccess)
        {
            _logger.LogWarning("Failed to activate person with ID {PersonId}: {ErrorMessage}", id, result.ErrorMessage);
            if (result.ErrorMessage == "Person not found.")
                return NotFound(new { message = result.ErrorMessage });
            return BadRequest(new { message = result.ErrorMessage });
        }

        _logger.LogInformation("Person with ID {PersonId} activated successfully", id);
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
        _logger.LogInformation("Deleting person with ID {PersonId}", id);
        var result = await _service.DeletePersonAsync(id);
        if (!result.IsSuccess)
        {
            _logger.LogWarning("Failed to delete person with ID {PersonId}: {ErrorMessage}", id, result.ErrorMessage);
            return NotFound(new { message = result.ErrorMessage });
        }

        _logger.LogInformation("Person with ID {PersonId} deleted successfully.", id);
        return Ok(new { message = "Person has been permanently deleted." });
    }
}