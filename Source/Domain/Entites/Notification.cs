using Domain.Common;
using NetTopologySuite.Geometries;

namespace Domain.Entites;

public class Notification : BaseAuditableEntity
{
    public string Message { get; set; } = string.Empty;
    public Point Location { get; set; } = Point.Empty;

    public override string ToString() => $"{Id} {Location.X} {Location.Y} {Message}";
}