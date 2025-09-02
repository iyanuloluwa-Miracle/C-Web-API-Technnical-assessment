using AutoMapper;
using Application.DTOs;
using Application.Profiles;
using Application.Services;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests;

public class ProductServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly IMapper _mapper;
    private readonly ProductService _productService;

    public ProductServiceTests()
    {
        // Setup AutoMapper
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });
        _mapper = mapperConfig.CreateMapper();

        // Setup mocks
        _mockProductRepository = new Mock<IProductRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockUnitOfWork.Setup(uow => uow.Products).Returns(_mockProductRepository.Object);

        // Create service with mocks
        _productService = new ProductService(_mockUnitOfWork.Object, _mapper);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllProducts()
    {
        // Arrange
        var products = new List<Product>
        {
            new Product { Id = 1, Name = "Product 1", Description = "Description 1", Price = 10.99m, StockQuantity = 100 },
            new Product { Id = 2, Name = "Product 2", Description = "Description 2", Price = 20.99m, StockQuantity = 200 }
        };

        _mockProductRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(products);

        // Act
        var result = await _productService.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Equal("Product 1", result.First().Name);
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ShouldReturnProduct()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "Product 1", Description = "Description 1", Price = 10.99m, StockQuantity = 100 };
        _mockProductRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(product);

        // Act
        var result = await _productService.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Product 1", result.Name);
    }

    [Fact]
    public async Task AddAsync_ShouldAddProduct()
    {
        // Arrange
        var productDto = new ProductDto { Name = "New Product", Description = "Description", Price = 15.99m, StockQuantity = 50 };
        
        // Act
        await _productService.AddAsync(productDto);

        // Assert
        _mockProductRepository.Verify(repo => repo.AddAsync(It.IsAny<Product>()), Times.Once);
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithValidProduct_ShouldUpdateProduct()
    {
        // Arrange
        var existingProduct = new Product { Id = 1, Name = "Old Name", Description = "Old Description", Price = 10.99m, StockQuantity = 100 };
        var updatedDto = new ProductDto { Id = 1, Name = "Updated Name", Description = "Updated Description", Price = 19.99m, StockQuantity = 150 };

        _mockProductRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(existingProduct);

        // Act
        await _productService.UpdateAsync(updatedDto);

        // Assert
        _mockProductRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Product>()), Times.Once);
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithInvalidId_ShouldThrowException()
    {
        // Arrange
        var updatedDto = new ProductDto { Id = 999, Name = "Updated Name", Description = "Updated Description", Price = 19.99m, StockQuantity = 150 };
        _mockProductRepository.Setup(repo => repo.GetByIdAsync(999)).ReturnsAsync((Product?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _productService.UpdateAsync(updatedDto));
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteProduct()
    {
        // Arrange
        int productId = 1;

        // Act
        await _productService.DeleteAsync(productId);

        // Assert
        _mockProductRepository.Verify(repo => repo.DeleteAsync(productId), Times.Once);
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
    }
}