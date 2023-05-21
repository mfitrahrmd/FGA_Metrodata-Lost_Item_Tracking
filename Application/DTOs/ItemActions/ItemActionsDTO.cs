namespace Application.DTOs.ItemActions;

public class ItemActionsDTO
{
    public Guid Id { get; set; }
    public DateTime Time { get; set; }
    
    public Guid ItemId { get; set; }
    public Guid ActionId { get; set; }
}