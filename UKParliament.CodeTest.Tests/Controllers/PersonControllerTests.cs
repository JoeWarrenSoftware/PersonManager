using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UKParliament.CodeTest.Services;
using UKParliament.CodeTest.Web.Controllers;
using UKParliament.CodeTest.Web.Contracts.Requests;
using Xunit;
using FluentValidation;
using FluentValidation.Results;
using UKParliament.CodeTest.Services.Helpers;
using Microsoft.Extensions.Logging;
using UKParliament.CodeTest.Data;

namespace UKParliament.CodeTest.Tests.Controllers;
public class PersonControllerTests
{
    private readonly Mock<IPersonService> _serviceMock;
    private readonly Mock<ILogger<PersonController>> _loggerMock;
    private readonly Mock<IValidator<CreatePersonRequest>> _createValidatorMock;
    private readonly Mock<IValidator<UpdatePersonRequest>> _updateValidatorMock;
    private readonly PersonController _controller;

    public PersonControllerTests()
    {
        _serviceMock = new Mock<IPersonService>();
        _loggerMock = new Mock<ILogger<PersonController>>();
        _createValidatorMock = new Mock<IValidator<CreatePersonRequest>>();
        _updateValidatorMock = new Mock<IValidator<UpdatePersonRequest>>();
        _controller = new PersonController(_serviceMock.Object, _loggerMock.Object, _createValidatorMock.Object, _updateValidatorMock.Object);
    }

    [Fact]
    public async Task GetById_ShouldReturnNotFound_WhenPersonDoesNotExist()
    {
        // Arrange
        var personId = 999;
        _serviceMock.Setup(s => s.GetPersonByIdAsync(personId))
                    .ReturnsAsync(ServiceResult<Person>.Failure("Person not found."));

        // Act
        var result = await _controller.GetById(personId);

        // Assert
        result.Result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task CreatePerson_ShouldReturnBadRequest_WhenValidationFails()
    {
        // Arrange
        var request = new CreatePersonRequest { FirstName = "", LastName = "Doe", DateOfBirth = DateTime.Now, DepartmentId = 1 };
        _createValidatorMock.Setup(v => v.ValidateAsync(request, default))
                            .ReturnsAsync(new ValidationResult(new[] { new ValidationFailure("FirstName", "First name is required.") }));

        // Act
        var result = await _controller.CreatePerson(request);

        // Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }
}
