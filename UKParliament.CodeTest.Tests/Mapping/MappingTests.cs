using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Web.Mapping;
using Xunit;

namespace UKParliament.CodeTest.Tests.Mapping;
public class MappingTests
{
    [Fact]
    public void MapToResponse_ShouldMapPersonCorrectly()
    {
        // Arrange
        var person = new Person
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateTime(1990, 1, 1),
            DepartmentId = 1,
            Email = "john@example.com"
        };

        // Act
        var response = person.MapToResponse();

        // Assert
        Assert.Equal(1, response.Id);
        Assert.Equal("John", response.FirstName);
        Assert.Equal("Doe", response.LastName);
        Assert.Equal("john@example.com", response.Email);
    }
}
