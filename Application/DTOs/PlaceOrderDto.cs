using System.Collections.Generic;

namespace Application.DTOs;

public class PlaceOrderDto
{
    public List<OrderItemDto> Items { get; set; } = new();
}