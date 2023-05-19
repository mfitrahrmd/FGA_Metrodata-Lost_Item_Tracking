using Domain.Entities;

namespace Application.DAOs.Item;

public class ApprovedFoundItem : Domain.Entities.Item
{
    public DateTime FoundAt { get; set; }
    public Employee FoundBy { get; set; }
}