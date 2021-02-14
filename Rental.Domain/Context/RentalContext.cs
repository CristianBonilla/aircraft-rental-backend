using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Rental.Domain
{
    public partial class RentalContext : DbContext
    {
        public RentalContext(DbContextOptions<RentalContext> contextOptions) : base(contextOptions) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}
