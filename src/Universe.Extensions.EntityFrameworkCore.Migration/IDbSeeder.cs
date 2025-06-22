// Source reference from https://github.com/dotnet/eShop/tree/release/8.0

using Microsoft.EntityFrameworkCore;

namespace Universe.Extensions.EntityFrameworkCore.Migration;

public interface IDbSeeder<in TContext>
    where TContext : DbContext
{
    Task SeedAsync(TContext context);
}
