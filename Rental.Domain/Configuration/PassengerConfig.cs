using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Rental.Domain
{
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
