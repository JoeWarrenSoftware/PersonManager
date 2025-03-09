namespace UKParliament.CodeTest.Web.Contracts.Responses;

public class PersonResponse
{
    public required int Id { get; init; }

    public required string FirstName { get; init; } = string.Empty;

    public required string LastName { get; init; } = string.Empty;

    public required DateTime DateOfBirth { get; init; }

    public required int DepartmentId { get; init; }

    public required string DepartmentName { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;

    public string PhoneNumber { get; init; } = string.Empty;

    public string ProfileImageUrl { get; init; } = string.Empty;

    public bool IsActive { get; init; }
}