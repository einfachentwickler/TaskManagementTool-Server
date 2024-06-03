using AutoFixture;
using AutoFixture.AutoNSubstitute;
using FluentValidation;
using FluentValidation.TestHelper;
using NUnit.Framework;
using TaskManagementTool.BusinessLogic.Commands.Auth.Login.Models;
using TaskManagementTool.BusinessLogic.Commands.Auth.Login.Validation;
using TaskManagementTool.Common.Constants;
using TaskManagementTool.Common.Enums;

namespace BusinessLogic.UnitTests.Commands.Auth.Login.Validation;

[TestFixture]
public class UserLoginRequestValidatorTests
{
    private const string VALID_EMAIL = "user1@email.com";
    private const string VALID_PASSWORD = "Qwerty123$";

    private IValidator<UserLoginRequest> sut;

    private IFixture fixture;

    [SetUp]
    public void Setup()
    {
        fixture = new Fixture().Customize(new AutoNSubstituteCustomization());

        sut = fixture.Create<UserLoginRequestValidator>();
    }

    [Test]
    public async Task ValidateAsync_ValidRequest_HasNoValidationErrors()
    {
        //Arrange
        UserLoginRequest request = new()
        {
            Email = VALID_EMAIL,
            Password = VALID_PASSWORD
        };

        //Act
        var actualResult = await sut.TestValidateAsync(request);

        //Assert
        actualResult.ShouldNotHaveAnyValidationErrors();
    }

    [TestCase(ValidationErrorCodes.EmptyValue, "")]
    [TestCase(ValidationErrorCodes.InvalidEmail, "not email at all")]
    public async Task ValidateAsync_InvalidRequest_HasValidationErrorForEmptyEmail(ValidationErrorCodes expectedResult, string email)
    {
        //Arrange
        UserLoginRequest request = new()
        {
            Email = email,
            Password = VALID_PASSWORD
        };

        //Act
        var actualResult = await sut.TestValidateAsync(request);

        //Assert
        actualResult.ShouldHaveValidationErrorFor(request => request.Email).WithErrorCode(expectedResult.ToString());
    }

    [Test]
    public async Task ValidateAsync_InvalidRequest_HasValidationErrorForTooBigEmail()
    {
        //Arrange
        UserLoginRequest request = new()
        {
            Email = new string('a', ValidationConstants.DEFAULT_TEXT_INPUT_SIZE + 1) + "@email.com",
            Password = VALID_PASSWORD
        };

        //Act
        var actualResult = await sut.TestValidateAsync(request);

        //Assert
        actualResult.ShouldHaveValidationErrorFor(request => request.Email).WithErrorCode(nameof(ValidationErrorCodes.TextLengthExceeded));
    }

    [Test]
    public async Task ValidateAsync_InvalidRequest_HasValidationErrorForEmptyPassword()
    {
        //Arrange
        UserLoginRequest request = new()
        {
            Email = VALID_EMAIL,
            Password = string.Empty
        };

        //Act
        var actualResult = await sut.TestValidateAsync(request);

        //Assert
        actualResult.ShouldHaveValidationErrorFor(request => request.Password).WithErrorCode(nameof(ValidationErrorCodes.EmptyValue));
    }
}
