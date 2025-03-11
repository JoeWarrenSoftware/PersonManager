using UKParliament.CodeTest.Data;

namespace UKParliament.CodeTest.Services;

public interface IDataStorageRepository
{
    Task<List<Person>> GetAllActivePeopleAsync();
    Task<Person?> GetPersonByIdAsync(int id);
    Task AddPersonAsync(Person person);
    Task UpdatePersonAsync(Person person);
    Task DeletePersonAsync(Person person);
    Task<List<Department>> GetAllDepartmentsAsync();
    Task<bool> DepartmentExistsAsync(int departmentId);
}