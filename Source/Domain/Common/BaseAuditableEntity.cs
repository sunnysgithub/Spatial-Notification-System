namespace Domain.Common;

public abstract class BaseAuditableEntity : BaseEntity
{
    public DateTime CreatedAtUtc { get; set; } = DateTime.MinValue;
    public Guid? CreatedBy { get; set; } = Guid.Empty;
    public DateTime ModifiedAtUtc { get; set; } = DateTime.MinValue;
    public Guid? ModifiedBy { get; set; } = Guid.Empty;
}