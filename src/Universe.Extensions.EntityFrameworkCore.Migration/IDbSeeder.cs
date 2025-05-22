// Source from https://github.com/dotnet/eShop

using Microsoft.EntityFrameworkCore;

namespace Universe.Extensions.EFCore.Migration;

public interface IDbSeeder<in TContext>
    where TContext : DbContext
{
    Task SeedAsync(TContext context);
}
