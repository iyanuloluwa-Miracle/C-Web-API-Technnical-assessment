using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services;

public class ProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProductDto>> GetAllAsync()
    {
        var products = await _unitOfWork.Products.GetAllAsync();
        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }

    public async Task<ProductDto?> GetByIdAsync(int id)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id);
        return _mapper.Map<ProductDto>(product);
    }

    public async Task AddAsync(ProductDto dto)
    {
        var product = _mapper.Map<Product>(dto);
        await _unitOfWork.Products.AddAsync(product);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateAsync(ProductDto dto)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(dto.Id);
        if (product == null) throw new KeyNotFoundException("Product not found");

        _mapper.Map(dto, product);
        await _unitOfWork.Products.UpdateAsync(product);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await _unitOfWork.Products.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<bool> PlaceOrderAsync(PlaceOrderDto order)
    {
        foreach (var item in order.Items)
        {
            var success = await _unitOfWork.Products.UpdateStockAsync(item.ProductId, item.Quantity);
            if (!success) return false;
        }
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}