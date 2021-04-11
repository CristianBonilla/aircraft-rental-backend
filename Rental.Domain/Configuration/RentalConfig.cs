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
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("NEWID()");
            builder.Property(p => p.Name)
                .HasMaxLength(100)
                .IsUnicode()
                .IsRequired();
            builder.Property(p => p.State)
                .IsRequired()
                .HasConversion(p => (char)p, p => (AircraftState)p);
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
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("NEWID()");
            builder.Property(p => p.Location)
                .HasColumnType("varchar(max)")
                .IsRequired();
            builder.Property(p => p.ArrivalDate)
                .IsRequired();
            builder.Property(p => p.DepartureDate)
                .IsRequired();
            builder.HasOne(o => o.Passenger)
                .WithMany()
                .HasForeignKey(f => f.PassengerId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(o => o.Aircraft)
                .WithMany()
                .HasForeignKey(f => f.AircraftId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    class PassengerConfig : IEntityTypeConfiguration<PassengerEntity>
    {
        public void Configure(EntityTypeBuilder<PassengerEntity> builder)
        {
            builder.ToTable("Passenger", "dbo")
                .HasKey(k => k.Id);
            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("NEWID()");
            builder.Property(p => p.IdentificationDocument)
                .IsRequired();
            builder.Property(p => p.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired();
            builder.Property(p => p.LastName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired();
            builder.Property(p => p.Specialty)
                .HasColumnType("varchar(max)")
                .IsRequired();
        }
    }
}
