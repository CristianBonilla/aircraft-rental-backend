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
            builder.HasIndex(i => i.Name)
                .IsUnique();
            builder.HasData(
                new { Id = 1, Name = DefaultRoles.AdminUser },
                new { Id = 2, Name = DefaultRoles.CommonUser });
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
            builder.HasData(
                new { Id = 1, Name = Permissions.ROLES },
                new { Id = 2, Name = Permissions.USERS },
                new { Id = 3, Name = Permissions.AIRCRAFTS },
                new { Id = 4, Name = Permissions.PASSENGERS },
                new { Id = 5, Name = Permissions.RENTALS });
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
            builder.HasData(
                new { IdRole = 1, IdPermission = 1 },
                new { IdRole = 1, IdPermission = 2 },
                new { IdRole = 1, IdPermission = 3 },
                new { IdRole = 1, IdPermission = 4 },
                new { IdRole = 1, IdPermission = 5 },
                new { IdRole = 2, IdPermission = 4 },
                new { IdRole = 2, IdPermission = 5 });
        }
    }
}
