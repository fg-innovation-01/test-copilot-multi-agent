using CopilotMultiAgent.Application.Services;
using CopilotMultiAgent.Domain.Interfaces.Repositories;
using CopilotMultiAgent.Infrastructure.Persistence;
using CopilotMultiAgent.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CopilotMultiAgent.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

        services.AddScoped<ITodoRepository, TodoRepository>();
        services.AddScoped<IScheduleRepository, ScheduleRepository>();
        services.AddScoped<ITodoService, TodoService>();
        services.AddScoped<IScheduleService, ScheduleService>();
        services.AddScoped<ISortingService, SortingService>();
        services.AddScoped<IMinMaxService, MinMaxService>();
        services.AddScoped<IFibonacciService, FibonacciService>();
        services.AddScoped<ISphereVolumeService, SphereVolumeService>();
        services.AddScoped<ICubeVolumeService, CubeVolumeService>();
        services.AddScoped<ICylinderVolumeService, CylinderVolumeService>();
        services.AddScoped<ITriangularPyramidVolumeService, TriangularPyramidVolumeService>();
        services.AddScoped<IRhombusService, RhombusService>();

        return services;
    }
}
