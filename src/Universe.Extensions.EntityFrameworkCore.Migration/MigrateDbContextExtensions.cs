// Source reference from https://github.com/dotnet/eShop/tree/release/8.0

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Universe.Extensions.EntityFrameworkCore.Migration;

#if NET8_0
public static class MigrateDbContextExtensions
{
    private const string ActivitySourceName = "DbMigrations";
    private static readonly ActivitySource ActivitySource = new(ActivitySourceName);

    [RequiresUnreferencedCode(
        "Entity Framework Core migrations may require unreferenced code for database providers and model building."
    )]
    [RequiresDynamicCode(
        "Entity Framework Core may require dynamic code generation for database providers and query compilation."
    )]
    public static IServiceCollection AddMigration<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TContext
    >(this IServiceCollection services)
        where TContext : DbContext => services.AddMigration<TContext>((_, _) => Task.CompletedTask);

    [RequiresUnreferencedCode(
        "Entity Framework Core migrations may require unreferenced code for database providers and model building."
    )]
    [RequiresDynamicCode(
        "Entity Framework Core may require dynamic code generation for database providers and query compilation."
    )]
    public static IServiceCollection AddMigration<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TContext
    >(this IServiceCollection services, Func<TContext, IServiceProvider, Task> seeder)
        where TContext : DbContext
    {
        return services.AddHostedService(sp => new MigrationHostedService<TContext>(sp, seeder));
    }

    [RequiresUnreferencedCode(
        "Entity Framework Core migrations may require unreferenced code for database providers and model building."
    )]
    [RequiresDynamicCode(
        "Entity Framework Core may require dynamic code generation for database providers and query compilation."
    )]
    public static IServiceCollection AddMigration<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TContext,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TDbSeeder
    >(this IServiceCollection services)
        where TContext : DbContext
        where TDbSeeder : class, IDbSeeder<TContext>
    {
        services.AddScoped<IDbSeeder<TContext>, TDbSeeder>();
        return services.AddMigration<TContext>(
            (context, sp) => sp.GetRequiredService<IDbSeeder<TContext>>().SeedAsync(context)
        );
    }

    [RequiresUnreferencedCode(
        "Entity Framework Core migrations may require unreferenced code for database providers and model building."
    )]
    [RequiresDynamicCode(
        "Entity Framework Core may require dynamic code generation for database providers and query compilation."
    )]
    private static async Task MigrateDbContextAsync<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TContext
    >(this IServiceProvider services, Func<TContext, IServiceProvider, Task> seeder)
        where TContext : DbContext
    {
        using var scope = services.CreateScope();
        var scopeServices = scope.ServiceProvider;
        var logger = scopeServices.GetRequiredService<ILogger<TContext>>();
        var context = scopeServices.GetService<TContext>();

        using var activity = ActivitySource.StartActivity(
            $"Migration operation {typeof(TContext).Name}"
        );

        if (context is not null)
        {
            try
            {
                logger.LogInformation(
                    "Migrating database associated with context {DbContextName}",
                    typeof(TContext).Name
                );

                var strategy = context.Database.CreateExecutionStrategy();

                await strategy.ExecuteAsync(() => InvokeSeeder(seeder, context, scopeServices));
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "An error occurred while migrating the database used on context {DbContextName}",
                    typeof(TContext).Name
                );
                activity?.SetExceptionTags(ex);

                throw;
            }
        }
    }

    [RequiresUnreferencedCode(
        "Entity Framework Core migrations may require unreferenced code for database providers and model building."
    )]
    [RequiresDynamicCode(
        "Entity Framework Core may require dynamic code generation for database providers and query compilation."
    )]
    private static async Task InvokeSeeder<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TContext
    >(Func<TContext, IServiceProvider, Task> seeder, TContext context, IServiceProvider services)
        where TContext : DbContext
    {
        using var activity = ActivitySource.StartActivity($"Migrating {typeof(TContext).Name}");

        try
        {
            var env = services.GetRequiredService<IHostEnvironment>();

            if (!env.IsDevelopment())
            {
                await context.Database.MigrateAsync();
            }

            await seeder(context, services);
        }
        catch (Exception ex)
        {
            activity?.SetExceptionTags(ex);

            throw;
        }
    }

    private class MigrationHostedService<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TContext
    >(IServiceProvider serviceProvider, Func<TContext, IServiceProvider, Task> seeder)
        : BackgroundService
        where TContext : DbContext
    {
        [UnconditionalSuppressMessage(
            "Trimming",
            "IL2026",
            Justification = "This library warns consumers about trimming requirements"
        )]
        [UnconditionalSuppressMessage(
            "AOT",
            "IL3050",
            Justification = "This library warns consumers about AOT requirements"
        )]
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return ExecuteMigrationAsync();
        }

        [RequiresUnreferencedCode(
            "Entity Framework Core migrations may require unreferenced code for database providers and model building."
        )]
        [RequiresDynamicCode(
            "Entity Framework Core may require dynamic code generation for database providers and query compilation."
        )]
        private Task ExecuteMigrationAsync()
        {
            return serviceProvider.MigrateDbContextAsync(seeder);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }
    }
}
#endif
