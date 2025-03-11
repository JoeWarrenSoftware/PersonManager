namespace UKParliament.CodeTest.Web.Contracts.Responses;

public class DepartmentResponse
{
    public required int Id { get; init; }

    public required string Name { get; init; } = string.Empty;
}
