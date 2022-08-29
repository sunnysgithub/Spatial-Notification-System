using Domain.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Persistence;
using NetTopologySuite.Geometries;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddApplicationServices();

builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "Spatial Notification System");

app.MapGet("/notifications", async (ApplicationDbContext db) => await db.Notifications.Select(n => n.ToString()).ToListAsync());

app.MapGet("/notifications/{guid}", async (Guid guid, ApplicationDbContext db) =>
{
    var notification = await db.Notifications.FirstOrDefaultAsync(n => n.Id.Equals(guid));
    return Results.Ok(notification.ToString());
});

app.MapPost("/notifications", async (CreateNotificationDto dto, ApplicationDbContext db) =>
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

public record CreateNotificationDto
{
    public string Message { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }
}