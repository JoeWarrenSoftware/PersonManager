using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Services.Helpers;

namespace UKParliament.CodeTest.Services;
public class DepartmentService : IDepartmentService
{
    private readonly IDataStorageRepository _repository;

    public DepartmentService(IDataStorageRepository repository)
    {
        _repository = repository;
    }

    public async Task<ServiceResult<List<Department>>> GetAllDepartmentsAsync()
    {
        var departments = await _repository.GetAllDepartmentsAsync();
        return ServiceResult<List<Department>>.Success(departments);
    }
}
