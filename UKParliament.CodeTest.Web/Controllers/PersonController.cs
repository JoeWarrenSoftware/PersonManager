using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Web.ViewModels;

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

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PersonViewModel>> GetById(int id)
    {
        var person = await _context.People
            .Include(p => p.Department)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (person == null)
        {
            return NotFound();
        }

        var personViewModel = new PersonViewModel
        {
            Id = person.Id,
            FirstName = person.FirstName,
            LastName = person.LastName,
            DateOfBirth = person.DateOfBirth,
            DepartmentId = person.DepartmentId,
            DepartmentName = person.Department?.Name?? "Unknown Department",
            Email = person.Email,
            PhoneNumber = person.PhoneNumber,
            ProfileImageUrl = person.ProfileImageUrl,
            IsActive = person.IsActive
        };

        return Ok(personViewModel);
    }

    [HttpGet]
    public async Task<ActionResult<List<PersonViewModel>>> GetAll()
    {
        var people = await _context.People
            .Include(p => p.Department)
            .ToListAsync();

        var peopleViewModel = people.Select(person => new PersonViewModel
        {
            Id = person.Id,
            FirstName = person.FirstName,
            LastName = person.LastName,
            DateOfBirth = person.DateOfBirth,
            DepartmentId = person.DepartmentId,
            DepartmentName = person.Department?.Name ?? "Unknown Department",
            Email = person.Email,
            PhoneNumber = person.PhoneNumber,
            ProfileImageUrl = person.ProfileImageUrl,
            IsActive = person.IsActive
        }).ToList();

        return Ok(peopleViewModel);
    }

    [HttpPost]
    public async Task<ActionResult<PersonViewModel>> CreatePerson([FromBody] PersonViewModel newPerson)
    {
        if (string.IsNullOrWhiteSpace(newPerson.FirstName) ||
            string.IsNullOrWhiteSpace(newPerson.LastName) ||
            newPerson.DepartmentId <= 0)
        {
            return BadRequest(new { message = "First Name, Last Name, and a valid Department are required." });
        }

        var department = await _context.Departments.FindAsync(newPerson.DepartmentId);
        if (department == null)
        {
            return BadRequest(new { message = "Invalid DepartmentId. Department does not exist." });
        }

        var person = new Person
        {
            FirstName = newPerson.FirstName,
            LastName = newPerson.LastName,
            DateOfBirth = newPerson.DateOfBirth,
            DepartmentId = newPerson.DepartmentId,
            Email = newPerson.Email,
            PhoneNumber = newPerson.PhoneNumber,
            ProfileImageUrl = newPerson.ProfileImageUrl,
            IsActive = newPerson.IsActive
        };

        _context.People.Add(person);
        await _context.SaveChangesAsync();

        var createdPerson = new PersonViewModel
        {
            Id = person.Id,
            FirstName = person.FirstName,
            LastName = person.LastName,
            DateOfBirth = person.DateOfBirth,
            DepartmentId = person.DepartmentId,
            DepartmentName = department.Name,
            Email = person.Email,
            PhoneNumber = person.PhoneNumber,
            ProfileImageUrl = person.ProfileImageUrl,
            IsActive = person.IsActive
        };

        return CreatedAtAction(nameof(GetById), new { id = createdPerson.Id }, createdPerson);
    }

}