using ArqEmCamadas.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ArqEmCamadas.Infra.ORM.Contexts;

public sealed class ApplicationContext(
    DbContextOptions<ApplicationContext> dbContext) 
    : IdentityDbContext<
    User,
    Role,
    Guid,
    UserClaim,
    UserRole,
    UserLogin,
    RoleClaim,
    UserToken>(dbContext)
{


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.ConfigureWarnings(warnings => 
            warnings.Ignore(SqlServerEventId.SavepointsDisabledBecauseOfMARS));
    }
}