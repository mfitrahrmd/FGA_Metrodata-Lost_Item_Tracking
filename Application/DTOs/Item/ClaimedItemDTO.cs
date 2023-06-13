using Application.DTOs.Employee;

namespace Application.DTOs.Item;

public class ClaimedItemDTO : ItemDTO
{
    public EmployeeDTO ClaimedBy { get; set; }
    public DateTime ClaimedAt { get; set; }
}