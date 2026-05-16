namespace food_heaven_backend.Shared.Domain.Repositories;

public interface IUnitOfWork
{
    Task CompleteAsync();
}