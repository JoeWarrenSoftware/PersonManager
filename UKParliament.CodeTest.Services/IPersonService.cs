using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Services.Helpers;

namespace UKParliament.CodeTest.Services;
public interface IPersonService
{
    Task<ServiceResult<List<Person>>> GetAllPeopleAsync();
    Task<ServiceResult<Person>> GetPersonByIdAsync(int id);
    Task<ServiceResult> AddPersonAsync(Person person);
    Task<ServiceResult> UpdatePersonAsync(Person person);
    Task<ServiceResult> DeactivatePersonAsync(int id);
    Task<ServiceResult> ActivatePersonAsync(int id);
    Task<ServiceResult> DeletePersonAsync(int id);
}