using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(
    opts => opts.UseNpgsql(
        builder.Configuration["ConnectionStrings:db"], 
        o => o.UseNetTopologySuite()
    )
);

builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/", () => "Spatial Notification System");

app.MapGet("/notifications", async (DataContext db) => await db.Notifications.Select(n => n.ToString()).ToListAsync());

app.MapGet("/notifications/{guid}", async (Guid guid, DataContext db) =>
{
    var notification = await db.Notifications.FirstOrDefaultAsync(n => n.Id.Equals(guid));
    return Results.Ok(notification.ToString());
});

app.MapPost("/notifications", async (CreateNotificationDto dto, DataContext db) =>
{
    var notification = db.Notifications.Add(new Notification()
    {
        Location = new Point(dto.Longitude, dto.Latitude),
        Message = dto.Message,
        CreatedAtUtc = DateTime.UtcNow,
        ModifiedAtUtc = DateTime.UtcNow,
    }).Entity;
    
    await db.SaveChangesAsync();
    return Results.Ok();
});

app.Run();

public abstract class BaseEntity
{
    public Guid Id { get; set; } = Guid.Empty;
    public Guid InstanceGuid { get; } = Guid.NewGuid();
}

public abstract class BaseAuditableEntity : BaseEntity
{
    public DateTime CreatedAtUtc { get; set; } = DateTime.MinValue;
    public Guid? CreatedBy { get; set; } = Guid.Empty;
    public DateTime ModifiedAtUtc { get; set; } = DateTime.MinValue;
    public Guid? ModifiedBy { get; set; } = Guid.Empty;
}

public class Notification : BaseAuditableEntity
{
    public string Message { get; set; } = string.Empty;
    public Point Location { get; set; } = Point.Empty;

    public override string ToString() => $"{Id} {Location.X} {Location.Y} {Message}";
}

public record CreateNotificationDto
{
    public string Message { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }
}

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options){}
    
    public DbSet<Notification> Notifications { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasPostgresExtension("postgis");
    }
}