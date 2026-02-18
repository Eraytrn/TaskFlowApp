using FluentValidation;
using MediatR;

namespace TaskFlow.Application.Common.Behaviors;

/// <summary>
/// MediatR Pipeline Behavior - Her Command/Query'den önce çalışır
/// FluentValidation validator'ları otomatik çalıştırır
/// Validation başarısız olursa exception fırlatır, handler'a gitmez
/// </summary>
public class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        // Validator yoksa direkt handler'a git
        if (!_validators.Any())
        {
            return await next();
        }

        // Validation context oluştur
        var context = new ValidationContext<TRequest>(request);

        // Tüm validator'ları paralel çalıştır
        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        // Hataları topla
        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .ToList();

        // Hata varsa exception fırlat
        if (failures.Count != 0)
        {
            throw new ValidationException(failures);
        }

        // Validation başarılı, handler'a devam et
        return await next();
    }
}