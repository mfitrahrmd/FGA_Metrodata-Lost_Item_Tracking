using Domain.Common;

namespace Domain.Entities;

public class Status : BaseEntity
{
    public string Name { get; set; }
    public string Message { get; set; }
    
    public Guid ItemActionsId { get; set; }

    public override Guid Id { get => ItemActionsId; set => ItemActionsId = value; }

    public virtual ItemActions? ItemActions { get; set; }
}