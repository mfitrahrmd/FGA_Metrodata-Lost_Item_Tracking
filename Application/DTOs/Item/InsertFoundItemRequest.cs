#region

using Microsoft.AspNetCore.Http;

#endregion

namespace Application.DTOs.Item;

public class InsertFoundItemRequest
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public IFormFile? File { get; set; }
}