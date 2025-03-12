using FluentAssertions;
using Moq;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Services;
using Xunit;

namespace UKParliament.CodeTest.Tests.Services;
public class PersonServiceTests
{
    private readonly Mock<IDataStorageRepository> _repositoryMock;
    private readonly PersonService _personService;

    public PersonServiceTests()
    {
        _repositoryMock = new Mock<IDataStorageRepository>();
        _personService = new PersonService(_repositoryMock.Object);
    }

    [Fact]
    public async Task GetPersonByIdAsync_ShouldReturnPerson_WhenPersonExists()
    {
        // Arrange
        var personId = 1;
        var mockPerson = new Person { Id = personId, FirstName = "John", LastName = "Doe", DateOfBirth = new DateTime(1990,10,15), DepartmentId = 1};
        _repositoryMock.Setup(repo => repo.GetPersonByIdAsync(personId)).ReturnsAsync(mockPerson);

        // Act
        var result = await _personService.GetPersonByIdAsync(personId);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(mockPerson);
    }

    [Fact]
    public async Task GetPersonByIdAsync_ShouldReturnError_WhenPersonDoesNotExist()
    {
        // Arrange
        var personId = 999;
        _ = _repositoryMock.Setup(repo => repo.GetPersonByIdAsync(personId)).ReturnsAsync(null as Person);

        // Act
        var result = await _personService.GetPersonByIdAsync(personId);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Person not found.");
    }
}
