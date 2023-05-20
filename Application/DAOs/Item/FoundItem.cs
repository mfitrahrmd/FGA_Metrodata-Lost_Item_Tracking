using Domain.Entities;

namespace Application.DAOs.Item;

public class FoundItem : Domain.Entities.Item
{
    public DateTime FoundAt { get; set; }
    public Employee FoundBy { get; set; }
}