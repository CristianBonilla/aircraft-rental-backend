using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Rental.Domain
{
    class AircraftConfig : IEntityTypeConfiguration<AircraftEntity>
    {
        public void Configure(EntityTypeBuilder<AircraftEntity> builder)
        {
            builder.ToTable("Aircraft", "dbo")
                .HasKey(k => k.Id);
            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();
            builder.Property(p => p.Name)
                .HasMaxLength(30)
                .IsUnicode()
                .IsRequired();
            builder.Property(p => p.State)
                .IsRequired();
            builder.Property(p => p.Description)
                .HasColumnType("nvarchar(max)");
        }
    }

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
            builder.HasOne(o => o.Aircraft)
                .WithMany()
                .HasForeignKey(f => f.IdAircraft)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }

    class PassengerConfig : IEntityTypeConfiguration<PassengerEntity>
    {
        public void Configure(EntityTypeBuilder<PassengerEntity> builder)
        {
            builder.ToTable("Passenger", "dbo")
                .HasKey(k => k.Id);
            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();
            builder.Property(p => p.IdentificationDocument)
                .IsRequired();
            builder.Property(p => p.FirstName)
                .HasMaxLength(30)
                .IsUnicode(false)
                .IsRequired();
            builder.Property(p => p.LastName)
                .HasMaxLength(30)
                .IsUnicode(false)
                .IsRequired();
            builder.Property(p => p.Specialty)
                .HasColumnType("varchar(max)")
                .IsRequired();
        }
    }
}
