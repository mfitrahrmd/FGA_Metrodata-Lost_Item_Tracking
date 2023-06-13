using Domain.Entities;

namespace Application.DAOs.Item;

public class ClaimedItem : Domain.Entities.Item
{
    public DateTime ClaimedAt { get; set; }
    public Employee ClaimedBy { get; set; }
}