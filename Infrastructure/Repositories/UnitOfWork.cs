using Domain.Interfaces;
using Infrastructure.Data; // Ensure AppDbContext is in Infrastructure.Data namespace
// If AppDbContext is not in Infrastructure.Data, update the using directive below:
// using YourNamespaceWhereAppDbContextIsDefined;

namespace Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    public IProductRepository Products { get; }

    public UnitOfWork(AppDbContext context, IProductRepository productRepository)
    {
        _context = context;
        Products = productRepository;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}