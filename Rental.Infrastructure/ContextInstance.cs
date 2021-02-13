using Microsoft.EntityFrameworkCore;

namespace Rental.Infrastructure
{
    public class ContextInstance : IContextInstance
    {
        public IRepositoryContext<TContext> GetContext<TContext>(TContext context) where TContext : DbContext
        {
            var repositoryContext = new RepositoryContext<TContext>(context);

            return repositoryContext;
        }
    }
}
