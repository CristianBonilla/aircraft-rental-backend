using Microsoft.EntityFrameworkCore;

namespace Rental.Domain
{
    public partial class RentalContext : DbContext
    {
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<RentalEntity> Rentals { get; set; }

        public RentalContext(DbContextOptions<RentalContext> contextOptions) : base(contextOptions) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PassengerConfig())
                .ApplyConfiguration(new RoleConfig())
                .ApplyConfiguration(new UserConfig())
                .ApplyConfiguration(new RentalConfig());
            // modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}
