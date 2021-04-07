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
            builder.Property(p => p.DisplayName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired();
            builder.HasIndex(i => new { i.Name, i.DisplayName })
                .IsUnique();
            builder.HasData(
                new { Id = 1, Name = DefaultRoles.AdminUser, DisplayName = "Administrador" },
                new { Id = 2, Name = DefaultRoles.PassengerUser, DisplayName = "Pasajero" },
                new { Id = 3, Name = DefaultRoles.PilotUser, DisplayName = "Piloto" });
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
            builder.Property(p => p.DisplayName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired();
            builder.HasIndex(i => new { i.Name, i.DisplayName })
                .IsUnique();
            builder.HasData(
                new { Id = 1, Name = Permissions.CanRoles, DisplayName = "Roles" },
                new { Id = 2, Name = Permissions.CanUsers, DisplayName = "Usuarios" },
                new { Id = 3, Name = Permissions.CanRentals, DisplayName = "Alquileres" },
                new { Id = 4, Name = Permissions.CanAircrafts, DisplayName = "Aeronaves" },
                new { Id = 5, Name = Permissions.CanPassengers, DisplayName = "Pasajeros" });
        }
    }

    class RolePermissionConfig : IEntityTypeConfiguration<RolePermissionEntity>
    {
        public void Configure(EntityTypeBuilder<RolePermissionEntity> builder)
        {
            builder.ToTable("RolePermission", "dbo")
                .HasKey(k => new { k.RoleId, k.PermissionId });
            builder.HasOne(o => o.Role)
                .WithMany(m => m.Permissions)
                .HasForeignKey(f => f.RoleId);
            builder.HasOne(o => o.Permission)
                .WithMany()
                .HasForeignKey(f => f.PermissionId);
            builder.HasData(
                new { RoleId = 1, PermissionId = 1 },
                new { RoleId = 1, PermissionId = 2 },
                new { RoleId = 1, PermissionId = 3 },
                new { RoleId = 1, PermissionId = 4 },
                new { RoleId = 1, PermissionId = 5 },
                new { RoleId = 2, PermissionId = 5 },
                new { RoleId = 3, PermissionId = 4 });
        }
    }
}
