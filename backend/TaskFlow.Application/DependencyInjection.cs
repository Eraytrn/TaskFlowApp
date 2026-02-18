using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TaskFlow.Application.Common.Behaviors;

namespace TaskFlow.Application;

/// <summary>
/// Application katmanının Dependency Injection konfigürasyonu
/// Program.cs'de builder.Services.AddApplication() ile çağrılır
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // AutoMapper - MappingProfile'ı otomatik bulur
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        // MediatR - Tüm IRequest ve IRequestHandler'ları otomatik bulur
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        // FluentValidation - Tüm AbstractValidator<T> sınıflarını bulur
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // MediatR Pipeline Behavior - Validation'ı otomatik çalıştırır
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}
