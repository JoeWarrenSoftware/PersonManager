using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Web.Contracts.Requests;
using UKParliament.CodeTest.Web.Contracts.Responses;

namespace UKParliament.CodeTest.Web.Mapping;

public static class DepartmentMapping
{
    public static DepartmentResponse MapToResponse(this Department department)
    {
        return new DepartmentResponse
        {
            Id = department.Id,
            Name = department.Name,
        };
    }

    public static DepartmentsResponse MapToResponse(this IEnumerable<Department> departments)
    {
        return new DepartmentsResponse
        {
            Items = departments.Select(MapToResponse)
        };
    }
}
