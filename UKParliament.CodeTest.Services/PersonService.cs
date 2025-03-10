using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Services.Helpers;

namespace UKParliament.CodeTest.Services;

/// <summary>
/// This managers business logic for People
/// </summary>
public class PersonService : IPersonService
{
    private readonly IDataStorageRepository _repository;

    public PersonService(IDataStorageRepository repository)
    {
        _repository = repository;
    }

    public async Task<ServiceResult<List<Person>>> GetAllPeopleAsync()
    {
        var people = await _repository.GetAllActivePeopleAsync();
        return ServiceResult<List<Person>>.Success(people);
    }

    public async Task<ServiceResult<Person>> GetPersonByIdAsync(int id)
    {
        var person = await _repository.GetPersonByIdAsync(id);
        if (person == null)
            return ServiceResult<Person>.Failure("Person not found.");

        return ServiceResult<Person>.Success(person);
    }

    public async Task<ServiceResult> AddPersonAsync(Person person)
    {
        if (!await _repository.DepartmentExistsAsync(person.DepartmentId))
            return ServiceResult.Failure("Invalid DepartmentId. Department does not exist.");

        await _repository.AddPersonAsync(person);
        return ServiceResult.Success();
    }

    public async Task<ServiceResult> UpdatePersonAsync(Person person)
    {
        var existingPerson = await _repository.GetPersonByIdAsync(person.Id);
        if (existingPerson == null)
            return ServiceResult.Failure("Person not found.");

        if (!await _repository.DepartmentExistsAsync(person.DepartmentId))
            return ServiceResult.Failure("Invalid DepartmentId. Department does not exist.");

        await _repository.UpdatePersonAsync(person);
        return ServiceResult.Success();
    }

    public async Task<ServiceResult> DeactivatePersonAsync(int id)
    {
        var person = await _repository.GetPersonByIdAsync(id);
        if (person == null)
        {
            return ServiceResult.Failure("Person not found.");
        }

        if (!person.IsActive)
        {
            return ServiceResult.Failure("Person is already deactivated.");
        }

        person.IsActive = false;
        await _repository.UpdatePersonAsync(person);
        return ServiceResult.Success();
    }

    public async Task<ServiceResult> ActivatePersonAsync(int id)
    {
        var person = await _repository.GetPersonByIdAsync(id);
        if (person == null)
        {
            return ServiceResult.Failure("Person not found.");
        }

        if (person.IsActive)
        {
            return ServiceResult.Failure("Person is already activated.");
        }

        person.IsActive = true;
        await _repository.UpdatePersonAsync(person);
        return ServiceResult.Success();
    }

    public async Task<ServiceResult> DeletePersonAsync(int id)
    {
        var person = await _repository.GetPersonByIdAsync(id);
        if (person == null)
        {
            return ServiceResult.Failure($"Person not found.");
        }
        await _repository.DeletePersonAsync(person);
        return ServiceResult.Success();
    }
}