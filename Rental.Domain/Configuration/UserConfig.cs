using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Rental.Domain
{
    class UserConfig : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.ToTable("User", "dbo")
                .HasKey(k => k.Id);
            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();
            builder.Property(p => p.IdentificationDocument)
                .IsRequired();
            builder.Property(p => p.Username)
                .HasMaxLength(100)
                .IsRequired();
            builder.Property(p => p.Password)
                .HasColumnType("varchar(max)")
                .IsRequired();
            builder.Property(p => p.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .IsRequired();
            builder.Property(p => p.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired();
            builder.Property(p => p.LastName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired();
            builder.HasOne(o => o.Role)
                .WithMany()
                .HasForeignKey(f => f.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex(i => new { i.IdentificationDocument, i.Username, i.Email })
                .IsUnique();
        }
    }
}
