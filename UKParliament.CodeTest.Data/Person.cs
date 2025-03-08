namespace UKParliament.CodeTest.Data;

public class Person
{
    public int Id { get; set; }

    public required string FirstName { get; set; } = string.Empty;

    public required string LastName { get; set; } = string.Empty;

    public required DateTime DateOfBirth { get; set; }

    public required int DepartmentId { get; set; }

    public Department? Department { get; set; }

    public required string Email { get; set; } = string.Empty;

    public required string PhoneNumber { get; set; } = string.Empty;

    public required string ProfileImageUrl { get; set; } = string.Empty;

    public required bool IsActive  { get; set; }
}