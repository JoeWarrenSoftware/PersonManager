using FluentValidation.TestHelper;
using UKParliament.CodeTest.Web.Contracts.Requests;
using UKParliament.CodeTest.Web.Validators;
using Xunit;

namespace UKParliament.CodeTest.Tests.Validators;
public class CreatePersonRequestValidatorTests
{
    private readonly CreatePersonRequestValidator _validator;

    public CreatePersonRequestValidatorTests()
    {
        _validator = new CreatePersonRequestValidator();
    }

    [Fact]
    public void Should_Have_Error_When_FirstName_Is_Empty()
    {
        var model = new CreatePersonRequest { FirstName = "", LastName = "Doe", DateOfBirth = DateTime.Now, DepartmentId = 1 };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(p => p.FirstName);
    }

    [Fact]
    public void Should_Pass_When_FirstName_Is_Valid()
    {
        var model = new CreatePersonRequest { FirstName = "John", LastName = "Doe", DateOfBirth = DateTime.Now, DepartmentId = 1 };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(p => p.FirstName);
    }
}