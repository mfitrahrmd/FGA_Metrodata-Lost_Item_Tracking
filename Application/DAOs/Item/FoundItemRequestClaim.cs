using Domain.Entities;

namespace Application.DAOs.Item;

public class FoundItemRequestClaim
{
    public Guid RequestId { get; set; }
    public Domain.Entities.Item RequestItem { get; set; }
    public DateTime RequestAt { get; set; }
    public Employee RequestBy { get; set; }
    public string Status { get; set; }
}