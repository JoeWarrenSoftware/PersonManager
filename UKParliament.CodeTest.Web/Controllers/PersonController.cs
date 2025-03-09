using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Web.Contracts.Requests;
using UKParliament.CodeTest.Web.Contracts.Responses;
using UKParliament.CodeTest.Web.Mapping;

namespace UKParliament.CodeTest.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonController : ControllerBase
{
    private readonly PersonManagerContext _context;

    public PersonController(PersonManagerContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Get a specific person
    /// </summary>
    /// <param name="id">ID of a person</param>
    /// <returns>404,200</returns>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<PersonResponse>> GetById(int id)
    {
        var person = await _context.People
            .Include(p => p.Department)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (person == null)
        {
            return NotFound();
        }

        var response = person.MapToResponse();

        return Ok(response);
    }

    /// <summary>
    /// Get all people
    /// </summary>
    /// <returns>200</returns>
    [HttpGet]
    public async Task<ActionResult<PersonsResponse>> GetAll()
    {
        var persons = await _context.People
            .Include(p => p.Department)
            .Where(p => p.IsActive)
            .ToListAsync();

        var personsResponse = persons.MapToResponse();

        return Ok(personsResponse);
    }

    /// <summary>
    /// Create a person
    /// </summary>
    /// <param name="newPerson">Request body of person parameters</param>
    /// <returns>400,201</returns>
    [HttpPost]
    public async Task<ActionResult<PersonResponse>> CreatePerson([FromBody] CreatePersonRequest request)
    {
        // Validate required parameters
        if (string.IsNullOrWhiteSpace(request.FirstName) ||
            string.IsNullOrWhiteSpace(request.LastName) ||
            request.DepartmentId <= 0)
        {
            return BadRequest(new { message = "First Name, Last Name, and a valid Department are required." });
        }
        
        // Validate Department
        var department = await _context.Departments.FindAsync(request.DepartmentId);
        if (department == null)
        {
            return BadRequest(new { message = "Invalid DepartmentId. Department does not exist." });
        }

        // Add them
        var newPerson = request.MapToPerson();
        _context.People.Add(newPerson);
        await _context.SaveChangesAsync();

        var newPersonResponse = newPerson.MapToResponse();

        return CreatedAtAction(nameof(GetById), new { id = newPerson.Id }, newPersonResponse);
    }

    /// <summary>
    /// Update a person
    /// </summary>
    /// <param name="id">ID of a person</param>
    /// <param name="updatedPerson">Request body of person parameters</param>
    /// <returns>400,404,200</returns>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<PersonResponse>> UpdatePerson(int id, [FromBody] UpdatePersonRequest request)
    {
        // Validate person exists
        var existingPerson = await _context.People.FindAsync(id);
        if (existingPerson == null)
            return NotFound(new { message = "Person not found." });

        // Validate Department exists if updating it
        if (existingPerson.DepartmentId != request.DepartmentId)
        {
            var department = await _context.Departments.FindAsync(request.DepartmentId);
            if (department == null)
                return BadRequest(new { message = "Invalid DepartmentId. Department does not exist." });
        }

        // Update person
        request.MapToExistingPerson(existingPerson);
        await _context.SaveChangesAsync();

        var response = existingPerson.MapToResponse();

        return Ok(response);
    }

    /// <summary>
    /// Soft-delete a person by setting the IsActive property false 
    /// </summary>
    /// <param name="id">ID of a person</param>
    /// <returns>400,404,200</returns>
    [HttpPatch("{id}/deactivate")]
    public async Task<IActionResult> DeactivatePerson(int id)
    {
        // Validate person exists
        var person = await _context.People.FindAsync(id);
        if (person == null)
        {
            return NotFound(new { message = "Person not found." });
        }

        // Validate existing IsActive status
        if (!person.IsActive)
        {
            return BadRequest(new { message = "Person is already deactivated." });
        }

        // Soft delete: Set IsActive = false
        person.IsActive = false;
        await _context.SaveChangesAsync();

        return Ok(new { message = "Person has been deactivated." });
    }

    /// <summary>
    /// Reinstate a person by setting the IsActive property true 
    /// </summary>
    /// <param name="id">ID of person</param>
    /// <returns>400,404,200</returns>
    [HttpPatch("{id}/activate")]
    public async Task<IActionResult> ActivatePerson(int id)
    {
        // Validate person exists
        var person = await _context.People.FindAsync(id);
        if (person == null)
        {
            return NotFound(new { message = "Person not found." });
        }

        // Validate existing IsActive status
        if (person.IsActive)
        {
            return BadRequest(new { message = "Person is already active." });
        }

        // Activate status
        person.IsActive = true;
        await _context.SaveChangesAsync();

        return Ok(new { message = "Person has been reactivated." });
    }

    /// <summary>
    /// Hard-delete a person permanently from the datastore
    /// </summary>
    /// <param name="id">ID of a person</param>
    /// <returns>404,200</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePerson(int id)
    {
        var person = await _context.People.FindAsync(id);
        if (person == null)
        {
            return NotFound(new { message = "Person not found." });
        }
        var firstName = person.FirstName;
        var lastName = person.LastName;

        _context.People.Remove(person);
        await _context.SaveChangesAsync();

        return Ok(new { message = $"Person ({firstName} {lastName}) has been permanently deleted." });
    }
}