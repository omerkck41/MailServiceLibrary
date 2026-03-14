using FluentValidation;
using MailService.Abstractions;
using MailService.Domain;
using MailService.Providers;
using MailService.Services;
using MailService.Validations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MailService.Extensions;

/// <summary>
/// Her projeye (Clean Architecture, DDD vb.) kolayca dahil edilmesi için DI uzantısı.
/// </summary>
public static class MailServiceExtensions
{
    private static IServiceCollection AddCoreMailServices(this IServiceCollection services)
    {
        services.AddScoped<IValidator<EmailMessage>, EmailMessageValidator>();
        services.AddScoped<IMailService, MailManager>();
        return services;
    }

    /// <summary>
    /// Ücretsiz/SMTP sağlayıcısı ile mail servisini kaydeder.
    /// </summary>
    public static IServiceCollection AddSmtpMailService(this IServiceCollection services, Action<SmtpOptions> configure)
    {
        var options = new SmtpOptions();
        configure(options);

        services.AddSingleton<IMailProvider>(sp => new SmtpMailProvider(
            options.Host, 
            options.Port, 
            options.UserName, 
            options.Password, 
            sp.GetRequiredService<ILogger<SmtpMailProvider>>()));

        return services.AddCoreMailServices();
    }

    /// <summary>
    /// Profesyonel/SendGrid API sağlayıcısı ile mail servisini kaydeder.
    /// </summary>
    public static IServiceCollection AddSendGridMailService(this IServiceCollection services, string apiKey)
    {
        services.AddSingleton<IMailProvider>(sp => new SendGridMailProvider(
            apiKey, 
            sp.GetRequiredService<ILogger<SendGridMailProvider>>()));

        return services.AddCoreMailServices();
    }
}

public class SmtpOptions
{
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; } = 587;
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
