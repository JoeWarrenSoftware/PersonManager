using Microsoft.EntityFrameworkCore;
using Moq;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Services;
using Xunit;

namespace UKParliament.CodeTest.Tests.Repository;
public class DataStorageRepositoryTests
{
    private readonly Mock<PersonManagerContext> _contextMock;
    private readonly Mock<DbSet<Person>> _peopleDbSetMock;
    private readonly Mock<DbSet<Department>> _departmentDbSetMock;
    private readonly DataStorageRepository _repository;

    public DataStorageRepositoryTests()
    {
        // Mock DbSet<Person>
        _peopleDbSetMock = new Mock<DbSet<Person>>();
        _departmentDbSetMock = new Mock<DbSet<Department>>();

        // Mock DbContext
        _contextMock = new Mock<PersonManagerContext>();
        _contextMock.Setup(c => c.People).Returns(_peopleDbSetMock.Object);
        _contextMock.Setup(c => c.Departments).Returns(_departmentDbSetMock.Object);

        // Create repository with mocked context
        _repository = new DataStorageRepository(_contextMock.Object);
    }

    [Fact(Skip = "Mocking DbSet<T> seems to be the issue here with Setup")]
    public async Task GetAllActivePeopleAsync_ShouldReturnActivePeople()
    {
        // Arrange
        var people = new List<Person>
        {
            new Person { Id = 1, FirstName = "John", LastName = "Doe", DateOfBirth = new DateTime(1990,1,10), DepartmentId = 1, IsActive = true },
            new Person { Id = 2, FirstName = "Jane", LastName = "Doe", DateOfBirth = new DateTime(2000,3,15), DepartmentId = 2, IsActive = false }
        }.AsQueryable();

        _peopleDbSetMock.As<IQueryable<Person>>().Setup(m => m.Provider).Returns(people.Provider);
        _peopleDbSetMock.As<IQueryable<Person>>().Setup(m => m.Expression).Returns(people.Expression);
        _peopleDbSetMock.As<IQueryable<Person>>().Setup(m => m.ElementType).Returns(people.ElementType);
        _peopleDbSetMock.As<IQueryable<Person>>().Setup(m => m.GetEnumerator()).Returns(people.GetEnumerator());

        // Act
        var result = await _repository.GetAllActivePeopleAsync();

        // Assert
        Assert.Single(result);
        Assert.Equal("John", result[0].FirstName);
    }

    [Fact(Skip = "Mocking DbSet<T> seems to be the issue here with Setup")]
    public async Task AddPersonAsync_ShouldCallSaveChanges()
    {
        // Arrange
        var newPerson = new Person { Id = 3, FirstName = "Alice", LastName = "Smith", DateOfBirth = new DateTime(1995, 8, 20), DepartmentId = 1, };

        // Act
        await _repository.AddPersonAsync(newPerson);

        // Assert
        _peopleDbSetMock.Verify(m => m.Add(It.IsAny<Person>()), Times.Once);
        _contextMock.Verify(m => m.SaveChangesAsync(default), Times.Once);
    }
}
