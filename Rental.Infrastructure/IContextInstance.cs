using Microsoft.EntityFrameworkCore;

namespace Rental.Infrastructure
{
    public interface IContextInstance
    {
        IRepositoryContext<TContext> GetContext<TContext>(TContext context) where TContext : DbContext;
    }
}
