using Microsoft.EntityFrameworkCore;
using UKParliament.CodeTest.Data;

namespace UKParliament.CodeTest.Services;

/// <summary>
/// This manages data storage
/// </summary>
public class DataStorageRepository : IDataStorageRepository
{
    private readonly PersonManagerContext _context;

    public DataStorageRepository(PersonManagerContext context)
    {
        _context = context;
    }

    #region Person

    public async Task<List<Person>> GetAllActivePeopleAsync()
    {
        return await _context.People
            .Include(p => p.Department)
            .Where(p => p.IsActive)
            .ToListAsync();
    }

    public async Task<Person?> GetPersonByIdAsync(int id)
    {
        return await _context.People
            .Include(p => p.Department)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task AddPersonAsync(Person person)
    {
        _context.People.Add(person);
        await _context.SaveChangesAsync();
    }

    public async Task UpdatePersonAsync(Person person)
    {
        var existingPerson = await _context.People.FindAsync(person.Id);
        if (existingPerson != null)
        {
            _context.Entry(existingPerson).CurrentValues.SetValues(person);
        }
        await _context.SaveChangesAsync();
    }

    public async Task DeletePersonAsync(Person person)
    {
        _context.People.Remove(person);
        await _context.SaveChangesAsync();
    }

    #endregion

    #region Department

    public async Task<List<Department>> GetAllDepartmentsAsync()
    {
        return await _context.Departments
            .ToListAsync();
    }

    public async Task<bool> DepartmentExistsAsync(int departmentId)
    {
        return await _context.Departments.AnyAsync(d => d.Id == departmentId);
    }

    #endregion
}