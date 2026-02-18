using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Persistence;

// Persistence katmanının DI registration'ları
public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // DbContext'i DI container'a ekle
        services.AddDbContext<TaskFlowDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(TaskFlowDbContext).Assembly.FullName)));

        // Unit of Work'ü ekle
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}