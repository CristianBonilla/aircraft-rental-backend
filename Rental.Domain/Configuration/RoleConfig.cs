using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Rental.Domain
{
    abstract class DefaultValues
    {
        public readonly Guid AdminUserId = new Guid("6BBE4B56-3F81-4957-A8F1-33C9112DB4A2");
        public readonly Guid CommonUserId = new Guid("22B20E06-F147-41D6-8333-7C921242AD27");
        public readonly Guid PassengerUserId = new Guid("AEDB18FC-7B6C-488C-80BF-8BC2B36FEBE3");
        public readonly Guid PilotUserId = new Guid("DA9FBF03-D19B-4586-A28B-7B8DEAA7A5B6");

        public readonly Guid RolesPermissionId = new Guid("C5E3A53F-CE37-4512-91F3-A6D823DABE06");
        public readonly Guid UsersPermissionId = new Guid("B8C5CAA1-4A44-4783-AF7E-EB29617A5A70");
        public readonly Guid RentalsPermissionId = new Guid("186DF72B-0328-4539-8015-2965EB13CCEC");
        public readonly Guid AircraftsPermissionId = new Guid("44EB6612-536E-46D2-96EF-A752691F2296");
        public readonly Guid PassengersPermissionId = new Guid("352DEC26-951C-4236-AFB5-B059F014E819");
    }

    class RoleConfig : DefaultValues, IEntityTypeConfiguration<RoleEntity>
    {
        public void Configure(EntityTypeBuilder<RoleEntity> builder)
        {
            builder.ToTable("Role", "dbo")
                .HasKey(k => k.Id);
            builder.Property(p => p.Id)
                .HasDefaultValueSql("NEWID()");
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
                new { Id = AdminUserId, Name = DefaultRoles.AdminUser, DisplayName = "Administrador" },
                new { Id = CommonUserId, Name = DefaultRoles.CommonUser, DisplayName = "Usuario Común" },
                new { Id = PassengerUserId, Name = DefaultRoles.PassengerUser, DisplayName = "Pasajero" },
                new { Id = PilotUserId, Name = DefaultRoles.PilotUser, DisplayName = "Piloto" });
        }
    }

    class PermissionConfig : DefaultValues, IEntityTypeConfiguration<PermissionEntity>
    {
        public void Configure(EntityTypeBuilder<PermissionEntity> builder)
        {
            builder.ToTable("Permission", "dbo")
                .HasKey(k => k.Id);
            builder.Property(p => p.Id)
                .HasDefaultValueSql("NEWID()");
            builder.Property(p => p.Order)
                .IsRequired();
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
                new { Id = RolesPermissionId, Order = 1, Name = Permissions.CanRoles, DisplayName = "Roles" },
                new { Id = UsersPermissionId, Order = 2, Name = Permissions.CanUsers, DisplayName = "Usuarios" },
                new { Id = RentalsPermissionId, Order = 3, Name = Permissions.CanRentals, DisplayName = "Alquileres" },
                new { Id = AircraftsPermissionId, Order = 4, Name = Permissions.CanAircrafts, DisplayName = "Aeronaves" },
                new { Id = PassengersPermissionId, Order = 5, Name = Permissions.CanPassengers, DisplayName = "Pasajeros" });
        }
    }

    class RolePermissionConfig : DefaultValues, IEntityTypeConfiguration<RolePermissionEntity>
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
                new { RoleId = AdminUserId, PermissionId = RolesPermissionId },
                new { RoleId = AdminUserId, PermissionId = UsersPermissionId },
                new { RoleId = AdminUserId, PermissionId = RentalsPermissionId },
                new { RoleId = AdminUserId, PermissionId = AircraftsPermissionId },
                new { RoleId = AdminUserId, PermissionId = PassengersPermissionId },

                new { RoleId = CommonUserId, PermissionId = RentalsPermissionId },
                new { RoleId = CommonUserId, PermissionId = PassengersPermissionId },

                new { RoleId = PassengerUserId, PermissionId = AircraftsPermissionId },
                new { RoleId = PassengerUserId, PermissionId = PassengersPermissionId },

                new { RoleId = PilotUserId, PermissionId = RentalsPermissionId },
                new { RoleId = PilotUserId, PermissionId = AircraftsPermissionId },
                new { RoleId = PilotUserId, PermissionId = PassengersPermissionId });
        }
    }
}
