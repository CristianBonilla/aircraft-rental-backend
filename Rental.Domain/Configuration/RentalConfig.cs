using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Rental.Domain
{
    class RentalConfig : IEntityTypeConfiguration<RentalEntity>
    {
        public void Configure(EntityTypeBuilder<RentalEntity> builder)
        {
            builder.ToTable("Rental", "dbo")
                .HasKey(k => k.Id);
            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();
            builder.Property(p => p.Location)
                .HasColumnType("varchar(max)")
                .IsRequired();
            builder.Property(p => p.ArrivalDate)
                .IsRequired();
            builder.Property(p => p.DepartureDate)
                .IsRequired();
            builder.HasOne(o => o.Passenger)
                .WithMany()
                .HasForeignKey(f => f.IdPassenger)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
