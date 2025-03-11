namespace UKParliament.CodeTest.Web.Contracts.Responses;

public class DepartmentsResponse
{
    public required IEnumerable<DepartmentResponse> Items { get; init; } = Enumerable.Empty<DepartmentResponse>();
    public int TotalCount => Items.Count();
}
