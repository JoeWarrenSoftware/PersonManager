namespace UKParliament.CodeTest.Web.Contracts.Requests;

public class UpdatePersonRequest
{
    public required string FirstName { get; init; } = string.Empty;

    public required string LastName { get; init; } = string.Empty;

    public required DateTime DateOfBirth { get; init; }

    public required int DepartmentId { get; init; }

    public string Email { get; init; } = string.Empty;

    public string PhoneNumber { get; init; } = string.Empty;

    public string ProfileImageUrl { get; init; } = string.Empty;

    public bool IsActive { get; init; }
}