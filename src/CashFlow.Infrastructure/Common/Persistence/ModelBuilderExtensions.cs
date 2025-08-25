using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.Common.Persistence;

public static class ModelBuilderExtensions
{
    public static ModelBuilder UseStringEnums(this ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var properties = entityType.ClrType.GetProperties()
                .Where(p => p.PropertyType.IsEnum);

            foreach (var property in properties)
            {
                modelBuilder.Entity(entityType.Name).Property(property.Name)
                    .HasConversion<string>();
            }
        }
        return modelBuilder;
    }

    public static ModelBuilder ConfigureIdentityTables(this ModelBuilder modelBuilder)
    {
        var schemaIdentity = "identity";

        modelBuilder.Entity<Domain.User.UserEntity>(b =>
        {
            b.HasMany(e => e.Accounts)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            b.ToTable(name: "User", schema: schemaIdentity);
        });
        modelBuilder.Entity<Microsoft.AspNetCore.Identity.IdentityRole<Guid>>(b =>
        {
            b.ToTable(name: "Role", schema: schemaIdentity);
        });
        modelBuilder.Entity<Microsoft.AspNetCore.Identity.IdentityUserRole<Guid>>(b =>
        {
            b.ToTable(name: "UserRole", schema: schemaIdentity);
        });
        modelBuilder.Entity<Microsoft.AspNetCore.Identity.IdentityUserClaim<Guid>>(b =>
        {
            b.ToTable(name: "UserClaim", schema: schemaIdentity);
        });
        modelBuilder.Entity<Microsoft.AspNetCore.Identity.IdentityUserLogin<Guid>>(b =>
        {
            b.ToTable(name: "UserLogin", schema: schemaIdentity);
        });
        modelBuilder.Entity<Microsoft.AspNetCore.Identity.IdentityRoleClaim<Guid>>(b =>
        {
            b.ToTable(name: "RoleClaim", schema: schemaIdentity);
        });
        modelBuilder.Entity<Microsoft.AspNetCore.Identity.IdentityUserToken<Guid>>(b =>
        {
            b.ToTable(name: "UserToken", schema: schemaIdentity);
        });
        return modelBuilder;
    }
}
