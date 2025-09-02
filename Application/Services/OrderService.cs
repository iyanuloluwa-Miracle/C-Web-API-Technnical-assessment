using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Application.Services;

public class OrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<int> PlaceOrderAsync(PlaceOrderDto orderDto)
    {
        // Create a new order
        var order = new Order { OrderDate = DateTime.UtcNow };

        try
        {
            // For each item in the order
            foreach (var item in orderDto.Items)
            {
                var product = await _unitOfWork.Products.GetByIdAsync(item.ProductId);
                if (product == null)
                {
                    throw new KeyNotFoundException($"Product with ID {item.ProductId} not found");
                }

                if (product.StockQuantity < item.Quantity)
                {
                    throw new InvalidOperationException($"Insufficient stock for product {product.Name}");
                }

                // Create order item
                var orderItem = new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price
                };

                // Add to order
                order.Items.Add(orderItem);

                // Reduce stock
                product.StockQuantity -= item.Quantity;
                await _unitOfWork.Products.UpdateAsync(product);
            }

            // Add order to repository
            await _unitOfWork.Orders.AddAsync(order);
            
            // Save changes in a transaction
            await _unitOfWork.SaveChangesAsync();
            
            return order.Id;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            // Handle concurrency conflicts
            throw new InvalidOperationException("The order could not be placed due to a concurrency conflict. Please try again.", ex);
        }
    }

    public async Task<IEnumerable<dynamic>> GetAllOrdersAsync()
    {
        var orders = await _unitOfWork.Orders.GetAllAsync();
        
        return orders.Select(o => new
        {
            o.Id,
            o.OrderDate,
            o.Status,
            Items = o.Items.Select(i => new
            {
                ProductId = i.ProductId,
                ProductName = i.Product.Name,
                i.Quantity,
                i.UnitPrice,
                TotalPrice = i.Quantity * i.UnitPrice
            }),
            TotalAmount = o.Items.Sum(i => i.Quantity * i.UnitPrice)
        });
    }

    public async Task<dynamic?> GetOrderByIdAsync(int id)
    {
        var order = await _unitOfWork.Orders.GetByIdAsync(id);
        if (order == null) return null;

        return new
        {
            order.Id,
            order.OrderDate,
            order.Status,
            Items = order.Items.Select(i => new
            {
                ProductId = i.ProductId,
                ProductName = i.Product.Name,
                i.Quantity,
                i.UnitPrice,
                TotalPrice = i.Quantity * i.UnitPrice
            }),
            TotalAmount = order.Items.Sum(i => i.Quantity * i.UnitPrice)
        };
    }
}
