using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Rental.Domain
{
    class RoleConfig : IEntityTypeConfiguration<RoleEntity>
    {
        public void Configure(EntityTypeBuilder<RoleEntity> builder)
        {
            builder.ToTable("Role", "dbo")
                .HasKey(k => k.Id);
            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();
            builder.Property(p => p.Name)
                .HasMaxLength(30)
                .IsUnicode(false)
                .IsRequired();
        }
    }

    class PermissionConfig : IEntityTypeConfiguration<PermissionEntity>
    {
        public void Configure(EntityTypeBuilder<PermissionEntity> builder)
        {
            builder.ToTable("Permission", "dbo")
                .HasKey(k => k.Id);
            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();
            builder.Property(p => p.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired();
            builder.HasIndex(i => i.Name)
                .IsUnique();
        }
    }

    class RolePermissionConfig : IEntityTypeConfiguration<RolePermissionEntity>
    {
        public void Configure(EntityTypeBuilder<RolePermissionEntity> builder)
        {
            builder.ToTable("RolePermission", "dbo")
                .HasKey(k => new { k.IdRole, k.IdPermission });
            builder.HasOne(o => o.Role)
                .WithMany(m => m.Permissions)
                .HasForeignKey(f => f.IdRole);
            builder.HasOne(o => o.Permission)
                .WithMany()
                .HasForeignKey(f => f.IdPermission);
        }
    }
}
