using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    
    // Instead of using rowversion, we'll use a simple version number for concurrency in PostgreSQL
    [ConcurrencyCheck]
    public int Version { get; set; }
}