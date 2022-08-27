namespace Domain.Common;

public abstract class BaseEntity
{
    public Guid Id { get; set; } = Guid.Empty;
    public Guid InstanceGuid { get; } = Guid.NewGuid();
}