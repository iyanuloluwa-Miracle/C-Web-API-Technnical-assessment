namespace Domain.Interfaces;

public interface IUnitOfWork
{
    IProductRepository Products { get; }
    Task<int> SaveChangesAsync();
}