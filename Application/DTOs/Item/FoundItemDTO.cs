using Application.DTOs.Employee;

namespace Application.DTOs.Item;

public class FoundItemDTO : ItemDTO
{
    public EmployeeDTO FoundBy { get; set; }
    public DateTime FoundAt { get; set; }
}