using Domain.Entities;

namespace Application.DAOs.Item;

public class PendingFoundItemRequestClaim : Domain.Entities.Item
{
    public DateTime RequestAt { get; set; }
    public Employee RequestBy { get; set; }
}