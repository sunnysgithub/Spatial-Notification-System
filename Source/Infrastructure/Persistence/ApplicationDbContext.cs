using Domain.Entites;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){}
    
    public DbSet<Notification> Notifications { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasPostgresExtension("postgis");
    }
}