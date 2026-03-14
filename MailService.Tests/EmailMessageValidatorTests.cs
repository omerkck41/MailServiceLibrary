using FluentValidation.TestHelper;
using MailService.Domain;
using MailService.Validations;
using Xunit;

namespace MailService.Tests;

public class EmailMessageValidatorTests
{
    private readonly EmailMessageValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_From_Is_Invalid()
    {
        var model = new EmailMessage 
        { 
            From = "invalid-email", 
            To = ["test@test.com"], 
            Subject = "Test", 
            Body = "Test" 
        };
        
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.From);
    }

    [Fact]
    public void Should_Have_Error_When_To_Is_Empty()
    {
        var model = new EmailMessage 
        { 
            From = "test@test.com", 
            To = [], 
            Subject = "Test", 
            Body = "Test" 
        };
        
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.To);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Message_Is_Valid()
    {
        var model = new EmailMessage 
        { 
            From = "sender@test.com", 
            To = ["recipient@test.com"], 
            Subject = "Hello", 
            Body = "World" 
        };
        
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
