using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Web.Contracts.Requests;
using UKParliament.CodeTest.Web.Contracts.Responses;

namespace UKParliament.CodeTest.Web.Mapping;

public static class PersonMapping
{
    public static PersonResponse MapToResponse(this Person person)
    {
        return new PersonResponse
        {
            Id = person.Id,
            FirstName = person.FirstName,
            LastName = person.LastName,
            DateOfBirth = person.DateOfBirth,
            DepartmentId = person.DepartmentId,
            DepartmentName = person.Department?.Name ?? "Unknown Department",
            Email = person.Email,
            PhoneNumber = person.PhoneNumber,
            ProfileImageUrl = person.ProfileImageUrl,
            IsActive = person.IsActive
        };
    }

    public static PersonsResponse MapToResponse(this IEnumerable<Person> people)
    {
        return new PersonsResponse
        {
            Items = people.Select(MapToResponse)
        };
    }

    public static Person MapToPerson(this CreatePersonRequest request)
    {
        return new Person
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            DateOfBirth = request.DateOfBirth,
            DepartmentId = request.DepartmentId,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            ProfileImageUrl = request.ProfileImageUrl,
            IsActive = true
        };
    }

    public static Person MapToPerson(this UpdatePersonRequest request, int id)
    {
        return new Person
        {
            Id = id,
            FirstName = request.FirstName,
            LastName = request.LastName,
            DateOfBirth = request.DateOfBirth,
            DepartmentId = request.DepartmentId,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            ProfileImageUrl = request.ProfileImageUrl,
            IsActive = request.IsActive
        };
    }

    public static void MapToExistingPerson(this UpdatePersonRequest request, Person existingPerson)
    {
        existingPerson.FirstName = request.FirstName;
        existingPerson.LastName = request.LastName;
        existingPerson.DateOfBirth = request.DateOfBirth;
        existingPerson.DepartmentId = request.DepartmentId;
        existingPerson.Email = request.Email;
        existingPerson.PhoneNumber = request.PhoneNumber;
        existingPerson.ProfileImageUrl = request.ProfileImageUrl;
    }
}