using Application.DTOs.Item;
using Application.DTOs.Status;

namespace Application.DTOs.User;

public class MyRequestClaimResponse
{
    public ItemDTO Item { get; set; }

    public StatusDTO Status { get; set; }
}