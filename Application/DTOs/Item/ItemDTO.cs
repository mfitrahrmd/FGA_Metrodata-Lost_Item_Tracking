namespace Application.DTOs.Item;

public class ItemDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? ImagePath { get; set; }
    public Guid EmployeeId { get; set; }
}