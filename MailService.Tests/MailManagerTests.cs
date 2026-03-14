using Moq;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MailService.Abstractions;
using MailService.Domain;
using MailService.Services;
using Microsoft.Extensions.Logging;
using Xunit;

namespace MailService.Tests;

public class MailManagerTests
{
    private readonly Mock<IMailProvider> _mockProvider;
    private readonly Mock<IValidator<EmailMessage>> _mockValidator;
    private readonly Mock<ILogger<MailManager>> _mockLogger;
    private readonly MailManager _sut;

    public MailManagerTests()
    {
        _mockProvider = new Mock<IMailProvider>();
        _mockValidator = new Mock<IValidator<EmailMessage>>();
        _mockLogger = new Mock<ILogger<MailManager>>();
        
        // SUT (System Under Test)
        _sut = new MailManager(new[] { _mockProvider.Object }, _mockValidator.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task SendEmailAsync_ShouldCallProvider_WhenMessageIsValid()
    {
        // Arrange
        var email = new EmailMessage
        {
            From = "test@test.com",
            To = new List<string> { "recipient@test.com" },
            Subject = "Test Subject",
            Body = "Test Body"
        };

        _mockValidator.Setup(v => v.ValidateAsync(email, default))
                     .ReturnsAsync(new ValidationResult());

        _mockProvider.Setup(p => p.SendAsync(It.IsAny<EmailMessage>()))
                     .ReturnsAsync(true);

        // Act
        await _sut.SendEmailAsync(email);

        // Assert
        _mockProvider.Verify(p => p.SendAsync(email), Times.Once);
    }

    [Fact]
    public async Task SendEmailAsync_ShouldThrowValidationException_WhenMessageIsInvalid()
    {
        // Arrange
        var email = new EmailMessage { From = "bad", To = [], Subject = "", Body = "" };
        
        var failures = new List<ValidationFailure> { new("From", "Email is bad") };
        _mockValidator.Setup(v => v.ValidateAsync(email, default))
                     .ReturnsAsync(new ValidationResult(failures));

        // Act
        var act = () => _sut.SendEmailAsync(email);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task SendEmailAsync_ShouldThrowException_WhenNoProvidersConfigured()
    {
        // Arrange
        var emptySut = new MailManager(Enumerable.Empty<IMailProvider>(), _mockValidator.Object, _mockLogger.Object);
        var email = new EmailMessage
        {
            From = "test@test.com",
            To = new List<string> { "recipient@test.com" },
            Subject = "Test Subject",
            Body = "Test"
        };

        _mockValidator.Setup(v => v.ValidateAsync(email, default))
                     .ReturnsAsync(new ValidationResult());

        // Act
        var act = () => emptySut.SendEmailAsync(email);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
                 .WithMessage("Hiçbir mail sağlayıcısı yapılandırılmadı.");
    }
}
